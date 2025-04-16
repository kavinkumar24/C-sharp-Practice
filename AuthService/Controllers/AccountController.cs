using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AuthService.Models;

namespace AuthService.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterRequest model)
        {
            var user = new IdentityUser { UserName = model.UserName, Email = model.Email };
            if (string.IsNullOrWhiteSpace(model.Password))
            {
                return BadRequest("Password cannot be null or empty.");
            }

            if (model.Password != model.ConfirmPassword)
            {
                return BadRequest("Passwords do not match.");
            }
            var result = await _userManager.CreateAsync(user, model.Password);


            if (result.Succeeded)
            {
                return Content("User registered successfully!");
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest("Invalid login attempt.");
            }

            var result = await _userManager.CheckPasswordAsync(user, model.Password);
            if (result)
            {
                return Redirect("/home.html");
            }   

            return BadRequest("Invalid login attempt.");
        }
    }
}
