using ECommerce_WebSite.Areas.Admin.ViewModels;
using Entities.Models;
using Entities.UnitOfWork;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce_WebSite.Areas.Admin.Controllers
{
    // MUST specify the Area name
    [Area("Admin")]
    // Secure this controller so only users with the 'Admin' role can access it
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // ============================ Actions ============================
        /**
         * INDEX: List all categories
         */
        public IActionResult Index()
        {
            var categories = _unitOfWork.categoryRepo.GetAll().ToList();
            return View(categories);
        }

        /**
         * CREATE: Show form & Save new category
         */
        [HttpGet]
        public IActionResult Create()
        {
            // Pass all categories to the view in case we want to select a Parent Category
            ViewBag.CategoriesList = _unitOfWork.categoryRepo.GetAll().ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryVM model)
        {
            if(ModelState.IsValid)
            {
                // Map ViewModel to Entity
                var newCategory = new Category
                {
                    Name = model.Name,
                    ParentCategoryId = model.ParentCategoryId
                };
                _unitOfWork.categoryRepo.Add(newCategory);
                _unitOfWork.SaveTransact();
                TempData["SuccessMessage"] = "Category created successfully!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CategoriesList = _unitOfWork.categoryRepo.GetAll().ToList();
            return View(model);
        }

        /**
         * EDIT: Show form & Update category
         */
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _unitOfWork.categoryRepo.GetById(id);
            if (category == null)
                return NotFound();

            var model = new CategoryVM
            {
                Id = category.Id,
                Name = category.Name,
                ParentCategoryId = category.ParentCategoryId
            };
            // Pass other categories for the parent dropdown, excluding the current one
            ViewBag.CategoriesList = _unitOfWork.categoryRepo.FindAll(c => c.Id != id).ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryVM model)
        {
            if (ModelState.IsValid)
            {
                var category = _unitOfWork.categoryRepo.GetById(model.Id);
                if (category == null)
                    return NotFound();

                category.Name = model.Name;
                category.ParentCategoryId = model.ParentCategoryId;

                _unitOfWork.categoryRepo.Update(category);
                _unitOfWork.SaveTransact();

                TempData["SuccessMessage"] = "Category updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CategoriesList = _unitOfWork.categoryRepo.FindAll(c => c.Id != model.Id).ToList();
            return View(model);
        }

        /**
         * DELETE: Remove category
         */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            // Check if there are any products linked to this category before deleting
            var hasProducts = _unitOfWork.productRepo.FindAll(p => p.CategoryId == id).Any();
            if (hasProducts)
            {
                TempData["ErrorMessage"] = "Cannot delete this category because it contains products. Delete the products first.";
                return RedirectToAction(nameof(Index));
            }
            _unitOfWork.categoryRepo.Delete(id);
            _unitOfWork.SaveTransact();
            TempData["SuccessMessage"] = "Category deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
