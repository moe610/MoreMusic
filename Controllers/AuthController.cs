using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MoreMusic.DataLayer.Entity;
using MoreMusic.Models;
using MoreMusic.Services;

namespace MoreMusic.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<SystemUsers> _userManager;
        private readonly SignInManager<SystemUsers> _signInManager;
        private readonly AuthService _authService;

        public AuthController(UserManager<SystemUsers> userManager, SignInManager<SystemUsers> signInManager, AuthService authService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
        }

        /*
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return Unauthorized("Invalid login attempt.");

            var token = await _authService.GenerateJwtToken(user);
            return Ok(new { Token = token });
        }

        */
    }
}
