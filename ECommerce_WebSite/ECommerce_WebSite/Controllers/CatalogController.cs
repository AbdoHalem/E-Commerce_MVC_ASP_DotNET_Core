using ECommerce_WebSite.Models.ViewModels;
using Entities.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce_WebSite.Controllers
{
    [AllowAnonymous]
    public class CatalogController : Controller
    {
        readonly IUnitOfWork _unitOfWork;
        // Inject the Unit of Work into the controller
        public CatalogController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        /**
         * GET: /Catalog/Index
         * Browses products, filters by category, searches, and paginates.
         */
        public IActionResult Index(int? categoryId, string q, string sort, int page = 1)
        {
            /**
             * q: Search Query
             */
            int pageSize = 6; // Number of products per page
            // 1. Get all active products using the UnitOfWork
            var productsQuery = _unitOfWork.productRepo.FindAll(p => p.IsActive).AsQueryable();

            // 2. Filter by Category if the user clicked on one
            if (categoryId.HasValue && categoryId > 0)
            {
                productsQuery = productsQuery.Where(p => p.CategoryId == categoryId.Value);
            }

            // 3. Search by name description (SearchQuery 'q')
            if (!string.IsNullOrEmpty(q))
            {
                // ToLower() helps with case-insensitive search
                productsQuery = productsQuery.Where(p => p.Name.ToLower().Contains(q.ToLower()) || p.Description.ToLower().Contains(q.ToLower()));
            }

            // 4. Sort the products
            switch (sort)
            {
                case "price_asc":
                    productsQuery = productsQuery.OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    productsQuery = productsQuery.OrderByDescending(p => p.Price);
                    break;
                case "name_asc":
                    productsQuery = productsQuery.OrderBy(p => p.Name);
                    break;
                default:    // Default sorting (Newest First)
                    productsQuery = productsQuery.OrderByDescending(p => p.Id); // Newest first
                    break;
            }

            // 5. Calculate pagination metrics BEFORE applying Skip and Take
            int totalItems = productsQuery.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            
            // 6. Skip the previous pages and take only the items for the current page
            var pagedProducts = productsQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ToList();

            // 7. Map to ProductListVM
            var viewModel = new ProductListVM
            {
                Products = pagedProducts,
                CurrentCategoryId = categoryId,
                SearchQuery = q,
                CurrentSort = sort,
                CurrentPage = page,
                TotalPages = totalPages,
                Categories = _unitOfWork.categoryRepo.GetAll().ToList() // Get categories for the sidebar filter
            };
            return View(viewModel);
        }

        /**
         * GET: /Catalog/Details/5
         */
        public IActionResult Details(int id)
        {
            // Fetch the product by ID
            var product = _unitOfWork.productRepo.GetById(id);
            // If product is not found or not active, return a 404 Not Found page
            if (product == null || !product.IsActive)
            {
                return NotFound();
            }
            // Fetch the category to get its name
            var category = _unitOfWork.categoryRepo.GetById(product.CategoryId);
            
            // Map the Entity to the ViewModel
            var viewModel = new ProductDetailsVM
            {
                Id = product.Id,
                Name = product.Name,
                SKU = product.SKU,
                Price = product.Price,
                Description = product.Description,
                StockQuantity = product.StockQuantity,
                CategoryName = category != null ? category.Name : "Uncategorized",
                ImageUrl = product.ImageUrl
            };
            return View(viewModel);
        }
    }
}
