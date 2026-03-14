using ECommerce_WebSite.Areas.Admin.ViewModels;
using Entities.Models;
using Entities.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce_WebSite.Areas.Admin.Controllers
{
    // Specify the Area and restrict access to Admins only
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        readonly IUnitOfWork _unitOfWork;
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // ======================== Actions ========================
        /**
         * INDEX: List all orders in the system
         */
        public IActionResult Index()
        {
            // Fetch all orders and order them by the most recent first
            var allOrders = _unitOfWork.orderRepo.GetAll().OrderByDescending(o => o.OrderDate).ToList();
            return View(allOrders);
        }

        /**
         * DETAILS: View order items and shipping info
         */
        [HttpGet]
        public IActionResult Details(int id)
        {
            var order = _unitOfWork.orderRepo.GetById(id);
            if (order == null)
                return NotFound();

            // Fetch related data
            var orderItems = _unitOfWork.orderItemRepo.FindAll(oi => oi.OrderId == id).ToList();
            var address = _unitOfWork.addressRepo.GetById(order.ShippingAddressId);

            //Map to ViewModel to avoid passing raw entities to the view
            var viewModel = new AdminOrderDetailsVM
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
         * UPDATE STATUS: Change order status
         */
        // ==========================================
        // UPDATE STATUS: Change order status
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStatus(int orderId, OrderStatus newStatus)
        {
            var order = _unitOfWork.orderRepo.GetById(orderId);
            if (order == null) return NotFound();

            // Business Logic: If the admin cancels the order, we must restore the stock
            if (newStatus == OrderStatus.Cancelled && order.Status != OrderStatus.Cancelled)
            {
                var orderItems = _unitOfWork.orderItemRepo.FindAll(oi => oi.OrderId == orderId).ToList();
                foreach (var item in orderItems)
                {
                    var product = _unitOfWork.productRepo.GetById(item.ProductId);
                    if (product != null)
                    {
                        // Return items to inventory
                        product.StockQuantity += item.Quantity;
                        _unitOfWork.productRepo.Update(product);
                    }
                }
            }
            // Update the status
            order.Status = newStatus;
            _unitOfWork.orderRepo.Update(order);
            // Save all changes in a single transaction
            _unitOfWork.SaveTransact();
            TempData["SuccessMessage"] = $"Order status successfully updated to {newStatus}!";

            // Redirect back to the details page of the same order
            return RedirectToAction(nameof(Details), new { id = orderId });
        }
    }
}
