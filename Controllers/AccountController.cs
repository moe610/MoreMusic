using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MoreMusic.DataLayer;
using MoreMusic.DataLayer.Entity;
using MoreMusic.Models;

namespace MoreMusic.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<SystemUsers> _userManager;
        private readonly SignInManager<SystemUsers> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly MusicDbContext _musicDbContext;

        public AccountController(UserManager<SystemUsers> userManager, SignInManager<SystemUsers> signInManager, ILogger<AccountController> logger, MusicDbContext musicDbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _musicDbContext = musicDbContext;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new SystemUsers { UserName = model.UserName, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Check if the user was created with a ConcurrencyStamp
                    var createdUser = await _userManager.FindByNameAsync(user.UserName);
                    if (createdUser != null)
                    {
                        // Optionally, log or debug the ConcurrencyStamp value
                        Console.WriteLine($"ConcurrencyStamp: {createdUser.ConcurrencyStamp}");
                    }

                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                    return View(model);
                }


                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                    return RedirectToAction("Index","Home");

                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToAction("Lockout");
                }
                else if (result.IsNotAllowed)
                {
                    _logger.LogWarning("User is not allowed to sign in.");
                    return RedirectToAction("NotAllowed");
                }
                else if (result.RequiresTwoFactor)
                {
                    _logger.LogWarning("Two-factor authentication required.");
                    return RedirectToAction("TwoFactorAuthentication");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(model);
        }
    }
}
