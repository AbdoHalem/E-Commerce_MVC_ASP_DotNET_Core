using ECommerce_WebSite.Areas.Admin.ViewModels;
using Entities.Models;
using Entities.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
namespace ECommerce_WebSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")] // Ensure only Admins can manage products
    public class ProductController : Controller
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IWebHostEnvironment _webHostEnvironment;
        // Inject both UnitOfWork and WebHostEnvironment
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        // ===================== Actions =====================
        /**
         * INDEX: List all products
         */
        public IActionResult Index()
        {
            var products = _unitOfWork.productRepo.GetAll().ToList();
            // Pass categories to the view using ViewBag to display category names instead of IDs
            ViewBag.Categories = _unitOfWork.categoryRepo.GetAll().ToList();
            return View(products);
        }

        /**
         * CREATE: Show form & Save new product
         */
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.CategoriesList = _unitOfWork.categoryRepo.GetAll().ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductVM model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = "/images/products/default.png"; // Default image
                // 1. Handle Image Upload if a file was selected
                if (model.ImageFile != null)
                {
                    // Get the path to wwwroot/images/products
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");

                    //// Create the folder if it doesn't exist
                    //Directory.CreateDirectory(uploadsFolder);

                    // Generate a unique filename using Guid to prevent overwriting files with the same name
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    
                    // Save the file to the server
                    using(var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        model.ImageFile.CopyTo(fileStream);
                    }
                    // Update the path to be saved in the database
                    uniqueFileName = "/images/products/" + uniqueFileName;
                }
                // 2. Map ViewModel to Entity
                var newProduct = new Product
                {
                    Name = model.Name,
                    SKU = model.SKU,
                    Price = model.Price,
                    StockQuantity = model.StockQuantity,
                    IsActive = model.IsActive,
                    Description = model.Description,
                    CategoryId = model.CategoryId,
                    ImageUrl = uniqueFileName,
                    CreatedAt = DateTime.Now
                };
                // 3. Save to database
                _unitOfWork.productRepo.Add(newProduct);
                _unitOfWork.SaveTransact();

                TempData["SuccessMessage"] = "Product created successfully!";
                return RedirectToAction(nameof(Index));
            }
            // If validation fails, reload the dropdown list
            ViewBag.CategoriesList = _unitOfWork.categoryRepo.GetAll().ToList();
            return View(model);
        }

        /**
         * EDIT: Show form & Update product
         */
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _unitOfWork.productRepo.GetById(id);
            if (product == null)
                return NotFound();

            var model = new ProductVM
            {
                Id = product.Id,
                Name = product.Name,
                SKU = product.SKU,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                IsActive = product.IsActive,
                Description = product.Description,
                CategoryId = product.CategoryId,
                ImageUrl = product.ImageUrl // Pass the existing image path to display it
            };
            ViewBag.CategoriesList = _unitOfWork.categoryRepo.GetAll().ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductVM model)
        {
            if(ModelState.IsValid)
            {
                var product = _unitOfWork.productRepo.GetById(model.Id);
                if(product == null)
                    return NotFound();

                // 1. Handle Image Update
                if (model.ImageFile != null)
                {
                    // Delete the old image if it's not the default one
                    if (!string.IsNullOrEmpty(product.ImageUrl) && product.ImageUrl != "/images/products/default.png")
                    {
                        string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, product.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    // Save the new image
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        model.ImageFile.CopyTo(fileStream);
                    }

                    // Update the entity with the new image path
                    product.ImageUrl = "/images/products/" + uniqueFileName;
                }
                // 2. Update other properties
                product.Name = model.Name;
                product.SKU = model.SKU;
                product.Price = model.Price;
                product.StockQuantity = model.StockQuantity;
                product.IsActive = model.IsActive;
                product.Description = model.Description;
                product.CategoryId = model.CategoryId;

                // 3. Save changes
                _unitOfWork.productRepo.Update(product);
                _unitOfWork.SaveTransact();

                TempData["SuccessMessage"] = "Product updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CategoriesList = _unitOfWork.categoryRepo.GetAll().ToList();
            return View(model);
        }

        /**
         * DELETE: Remove product
         */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var product = _unitOfWork.productRepo.GetById(id);
            if (product != null)
            {
                // Check if product is in any orders before deleting (Business Rule)
                bool hasOrders = _unitOfWork.orderItemRepo.FindAll(oi => oi.ProductId == id).Any();
                if (hasOrders)
                {
                    TempData["ErrorMessage"] = "Cannot delete this product because it has been ordered by customers. You can mark it as 'Inactive' instead.";
                    return RedirectToAction(nameof(Index));
                }
                // Delete the image file from the server
                if (!string.IsNullOrEmpty(product.ImageUrl) && product.ImageUrl != "/images/products/default.png")
                {
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, product.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }
                // Delete from DB
                _unitOfWork.productRepo.Delete(id);
                _unitOfWork.SaveTransact();
                TempData["SuccessMessage"] = "Product deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
