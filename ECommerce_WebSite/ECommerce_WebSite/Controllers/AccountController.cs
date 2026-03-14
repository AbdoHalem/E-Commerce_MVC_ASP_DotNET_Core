using ECommerce_WebSite.Models.ViewModels;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce_WebSite.Controllers
{
    public class AccountController : Controller
    {
        readonly UserManager<App_User> _userManager;
        readonly SignInManager<App_User> _signInManager;

        // Inject Identity managers
        public AccountController(UserManager<App_User> userManager, SignInManager<App_User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        // ========================= Actions =========================
        // ==========================================
        // REGISTER
        // ==========================================
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAsync(RegisterVM model)
        {
            if(ModelState.IsValid)
            {
                // 1. Create a new user object based on our App_User entity
                // We use Email as the UserName for easier login
                var user = new App_User
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName
                };

                // 2. Attempt to create the user in the database with the hashed password
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // 3. If successful, sign the user in automatically
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    // Redirect to the Catalog page after successful registration
                    return RedirectToAction("Index", "Catalog");
                }
                // 4. If creation fails (e.g., password not strong enough, email exists), add errors to ModelState
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // ==========================================
        // LOGIN
        // ==========================================

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            // Store the returnUrl in ViewData so we can use it after login
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAsync(LoginVM model, string returnUrl = null)
        {
            ViewData["returnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // 1. Attempt to sign in using the Email as the UserName
                // PasswordSignInAsync parameters: userName, password, isPersistent, lockoutOnFailure
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if(result.Succeeded)
                {
                    // 2. Check if the user was trying to access a secure page before logging in
                    if(!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        // Default redirect after successful login
                        return RedirectToAction("Index", "Catalog");
                    }
                }
                else
                {
                    // 3. Invalid credentials
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }
            // If something failed, redisplay form
            return View(model);
        }

        // ==========================================
        // LOGOUT
        // ==========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Sign the user out and clear the authentication cookie
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Catalog");
        }
    }
}
