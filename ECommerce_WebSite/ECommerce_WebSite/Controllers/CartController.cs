using ECommerce_WebSite.Extensions;
using ECommerce_WebSite.Models.ViewModels;
using Entities.Models;
using Entities.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce_WebSite.Controllers
{
    public class CartController : Controller
    {
        IUnitOfWork _unitOfWork;
        const String CartSessionKey = "MyCartSession";
        // Injecting the Unit of Work
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // =================== Actions ===================
        /**
         * GET: /Cart/Index
         * Displays the cart contents and totals.
         */
        public IActionResult Index()
        {
            // Retrieve cart from session, or create a new empty one if it doesn't exist
            var cart = HttpContext.Session.GetJson<List<CartItemVM>>(CartSessionKey) ?? new List<CartItemVM>();
            // Pass data to the view using the CartVM
            var viewModel = new CartVM
            {
                Items = cart
            };
            return View(viewModel);
        }

        /**
         * POST: /Cart/Add
         * Adds a product to the session cart.
         */
        [HttpPost]
        public IActionResult Add(int productId, int quantity = 1)
        {
            // 1. Fetch the product from the database using UnitOfWork
            Product product = _unitOfWork.productRepo.GetById(productId);
            if (product == null || !product.IsActive)
                return NotFound();

            // 2. Retrieve existing cart from session
            var cart = HttpContext.Session.GetJson<List<CartItemVM>>(CartSessionKey) ?? new List<CartItemVM>();
            
            // 3. Check if the product is already in the cart
            var existingItem = cart.FirstOrDefault(c => c.ProductId == productId);
            if(existingItem != null)
            {
                // If it exists, just increase the quantity (Check stock first!)
                if(existingItem.Quantity + quantity <= product.StockQuantity)
                {
                    existingItem.Quantity += quantity;
                }
                else
                {
                    TempData["Error"] = $"Sorry, only {product.StockQuantity} items available in stock.";
                    return RedirectToAction("Details", "Catalog", new { id = product });
                }
            }
            else
            {
                // If it's new, add it to the cart list
                if(quantity <= product.StockQuantity)
                {
                    cart.Add(new CartItemVM
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        Price = product.Price,
                        Quantity = quantity
                    });
                }
            }
            // 4. Save the updated cart back to the session
            HttpContext.Session.SetJson(CartSessionKey, cart);

            // Redirect to the Cart Index page to see the updated cart
            return RedirectToAction("Index");
        }

        /**
         * POST: /Cart/Update
         * Updates the quantity of a specific item in the cart.
         */
        [HttpPost]
        public IActionResult Update(int productId, int quantity)
        {
            var cart = HttpContext.Session.GetJson<List<CartItemVM>>(CartSessionKey);
            if(cart != null)
            {
                var item = cart.FirstOrDefault(c => c.ProductId == productId);
                if(item != null)
                {
                    // Optionally: Fetch product again to ensure they don't exceed stock limit
                    var product = _unitOfWork.productRepo.GetById(productId);
                    if(quantity <= product.StockQuantity && quantity > 0)
                    {
                        item.Quantity = quantity;
                        HttpContext.Session.SetJson(CartSessionKey, cart);
                    }
                }
            }
            return RedirectToAction("Index");
        }

        /**
         * POST: /Cart/Remove
         * Removes a specific item from the cart.
         */
        [HttpPost]
        public IActionResult Remove(int productId)
        {
            var cart = HttpContext.Session.GetJson<List<CartItemVM>>(CartSessionKey);
            if(cart != null )
            {
                // Remove all items that match the productId
                cart.RemoveAll(c => c.ProductId == productId);
                // Save back to session
                HttpContext.Session.SetJson(CartSessionKey, cart);
            }
            return RedirectToAction("Index");
        }
    }
}
