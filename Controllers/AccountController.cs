using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MoreMusic.DataLayer.Entity;
using MoreMusic.Models;
using System.Threading.Tasks;

namespace MoreMusic.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<SystemUsers> _userManager;

        public AccountController(UserManager<SystemUsers> userManager)
        {
            _userManager = userManager;
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
                    return RedirectToAction("Index","Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }
    }
}
