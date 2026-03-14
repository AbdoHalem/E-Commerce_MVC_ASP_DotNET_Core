using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce_WebSite.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        // Secure it for Admins only and specify the Area
        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
