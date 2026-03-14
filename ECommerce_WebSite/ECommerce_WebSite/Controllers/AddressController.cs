using ECommerce_WebSite.Models.ViewModels;
using Entities.Models;
using Entities.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce_WebSite.Controllers
{
    [Authorize] // Ensure only logged-in users can manage addresses
    public class AddressController : Controller
    {
        readonly IUnitOfWork _unitOfWork;
        public AddressController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /**
         * GET: /Address/Index
         * Displays a list of all saved addresses for the current user.
         */
        public IActionResult Index()
        {
            // Get the current logged-in user's ID
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Fetch addresses belonging to this user
            var addresses = _unitOfWork.addressRepo.FindAll(a => a.UserId == userId);
            return View(addresses);
        }

        /**
         * GET: /Address/Create
         * Displays the form to add a new address.
         */
        [HttpGet]
        public IActionResult Create(string returnUrl = null)
        {
            // Store the returnUrl in ViewData to pass it to the View
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        /**
         * POST: /Address/Create
         * Processes the new address form and saves it to the database.
         */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AddressVM model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                // Map the ViewModel to the Address Entity
                var newAddress = new Address
                {
                    UserId = userId,
                    Country = model.Country,
                    City = model.City,
                    Street = model.Street,
                    Zip = model.Zip,
                    IsDefault = model.IsDefault
                };
                // Add to repository and save transaction
                _unitOfWork.addressRepo.Add(newAddress);
                _unitOfWork.SaveTransact();

                // Redirect back to the checkout page
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                // Default redirect if there is no returnUrl
                return RedirectToAction("Index");
            }
            // 3. Keep the returnUrl even if validation fails
            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        /**
         * POST: /Address/Delete
         * Removes an address from the user's profile.
         */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var address = _unitOfWork.addressRepo.GetById(id);
            // Security check: Ensure the address exists and belongs to the user
            if(address !=  null && address.UserId == userId)
            {
                // Check if this address is used in any existing order
                bool isLinkedToOrder = _unitOfWork.orderRepo.FindAll(o => o.ShippingAddressId == id).Any();
                if (isLinkedToOrder)
                {
                    // Address is used, prevent deletion and show error message
                    TempData["ErrorMessage"] = "You cannot delete this address because it is linked to one of your previous orders. We need to keep it for your order history.";
                    return RedirectToAction("Index");
                }
                _unitOfWork.addressRepo.Delete(id);
                _unitOfWork.SaveTransact();
                TempData["SuccessMessage"] = "Address deleted successfully.";
            }

            return RedirectToAction("Index");
        }
    }
}
