using ECommerce_WebSite.Extensions;
using ECommerce_WebSite.Models.ViewModels;
using Entities.Models;
using Entities.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Claims;

namespace ECommerce_WebSite.Controllers
{
    [Authorize]  // Only logged-in users can access checkout and their orders
    public class OrderController : Controller
    {
        IUnitOfWork _unitOfWork;
        const string CartSessionKey = "MyCartSession";
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // ========================== Actions ==========================
        /**
         * GET: /Order/Index
         * Displays the list of orders for the current user.
         */
        public IActionResult Index()
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userOrders = _unitOfWork.orderRepo.FindAll(o => o.UserId == userId).OrderByDescending(o => o.OrderDate).ToList();
            return View(userOrders);
        }

        /**
         * GET: /Order/Details/5
         * Displays the details of a specific order.
         */
        public IActionResult Details(int id)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = _unitOfWork.orderRepo.GetById(id);
            // Security check: Ensure the order exists AND belongs to the logged-in user
            if (order == null || order.UserId != userId)
                return NotFound();

            var orderItems = _unitOfWork.orderItemRepo.FindAll(oi => oi.OrderId == id);
            var address = _unitOfWork.addressRepo.GetById(order.ShippingAddressId);

            var viewModel = new OrderDetailsVM
            {
                OrderId = order.Id,
                OrderNumber = order.OrderNumber,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                ShippingAddress = address,
                Items = orderItems
            };
            return View(viewModel);
        }

        /**
         * GET: /Order/Checkout
         * Displays the checkout page with the user's addresses and cart summary.
         */
        [HttpGet]
        public IActionResult Checkout()
        {
            var cart = HttpContext.Session.GetJson<List<CartItemVM>>(CartSessionKey);
            // If cart is empty, redirect back to Catalog
            if (cart == null || cart.Count == 0)
                return RedirectToAction("Index", "Catalog");

            // Get the ID of the currently logged-in user
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Fetch user addresses to show in the dropdown
            var userAddresses = _unitOfWork.addressRepo.FindAll(a => a.UserId == userId);

            var viewModel = new CheckoutVM
            {
                CartItems = cart,
                TotalAmount = cart.Sum(i => i.LineTotal),
                UserAddresses = userAddresses
            };
            return View(viewModel);
        }

        /**
         * POST: /Order/Checkout
         * Processes the order transaction securely.
         */
        [HttpPost]
        public IActionResult Checkout(CheckoutVM model)
        {
            var cart = HttpContext.Session.GetJson<List<CartItemVM>>(CartSessionKey);
            if (cart == null || cart.Count == 0)
                return RedirectToAction("Index", "Catalog");
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            model.UserAddresses = _unitOfWork.addressRepo.FindAll(a => a.UserId == userId);

            if (!ModelState.IsValid)
            {
                model.CartItems = cart;
                model.TotalAmount = cart.Sum(i => i.LineTotal);
                return View(model);
            }
            // ==========================================
            // ATOMIC TRANSACTION LOGIC START
            // ==========================================
            // Step 1: Validate stock for all items BEFORE creating anything
            foreach (var item in cart)
            {
                Product product = _unitOfWork.productRepo.GetById(item.ProductId);
                if(product == null || product.StockQuantity < item.Quantity)
                {
                    ModelState.AddModelError("", $"Sorry, the product{item.ProductName} does not have enough stock.");
                    model.CartItems = cart;
                    model.TotalAmount = cart.Sum(i => i.LineTotal);
                    return View(model);
                }
            }
            // Step 2: Create the Order Header
            Order newOrder = new Order
            {
                UserId = userId,
                ShippingAddressId = model.SelectedAddressId,
                // Generate a random 10-character unique order number (e.g. ORD-5A9F2B)
                OrderNumber = "ORD-" + Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper(),
                Status = OrderStatus.Pending,
                OrderDate = DateTime.Now,
                TotalAmount = cart.Sum(c => c.LineTotal)
            };
            // Note: EF Core tracks this, but doesn't save to DB yet!
            _unitOfWork.orderRepo.Add(newOrder);

            // Step 3 & 4: Create OrderItems and Decrease Product Stock
            foreach (var item in cart)
            {
                var product = _unitOfWork.productRepo.GetById(item.ProductId);
                // 4. Decrease stock
                product.StockQuantity -= item.Quantity;
                _unitOfWork.productRepo.Update(product);
                // 3. Create OrderItem
                Order_Item orderItem = new Order_Item
                {
                    Order = newOrder, // EF Core links this automatically using navigation properties
                    ProductId = item.ProductId,
                    UnitPrice = item.Price,
                    Quantity = item.Quantity,
                    LineTotal = item.LineTotal,
                };
                _unitOfWork.orderItemRepo.Add(orderItem);
            }
            // Step 5: Save changes inside ONE transaction!
            // If any of the above fails, the DB remains completely untouched.
            _unitOfWork.SaveTransact();

            // ==========================================
            // ATOMIC TRANSACTION LOGIC END
            // ==========================================
            // Clear the Cart after successful order
            HttpContext.Session.Remove(CartSessionKey);
            // Redirect to the customer's order history
            return RedirectToAction("Index");
        }

        /**
         * POST: /Order/Cancel
         * Cancels a pending order and restores the products stock.
         */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cancel(int id)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = _unitOfWork.orderRepo.GetById(id);
            // Security check: Order exists, belongs to the logged-in user, and is still Pending
            if(order != null && order.UserId == userId && order.Status == OrderStatus.Pending)
            {
                // 1. Change the order status to Cancelled (Enum value 5)
                order.Status = OrderStatus.Cancelled;
                _unitOfWork.orderRepo.Update(order);

                // 2. Restore the stock for each item in this order
                var orderItems = _unitOfWork.orderItemRepo.FindAll(oi => oi.OrderId == id);
                foreach (var item in orderItems)
                {
                    var product = _unitOfWork.productRepo.GetById(item.ProductId);
                    if (product != null)
                    {
                        // Add the quantity back to the inventory
                        product.StockQuantity += item.Quantity;
                        _unitOfWork.productRepo.Update(product);
                    }
                }
                // 3. Save all changes in one atomic transaction
                _unitOfWork.SaveTransact();
                TempData["SuccessMessage"] = "Your order has been cancelled successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Order cannot be cancelled. It might have already been processed or shipped.";
            }
            return RedirectToAction("Index");
        }
    }
}
