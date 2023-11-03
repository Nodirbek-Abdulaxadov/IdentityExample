using CustomizeIdentity.Identity;
using CustomizeIdentity.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;

namespace CustomizeIdentity.Controllers;

public class AuthController : Controller
{
    private readonly IUserInterface _userInterface;

    public AuthController(IUserInterface userInterface)
    {
        _userInterface = userInterface;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(AddUserViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var res = await _userInterface.CreateUserAsync(viewModel);

            if (res.IsSuccess)
            {
                return RedirectToAction("login");
            }

            ModelState.AddModelError("Email", res.ErrorMessage);
            return View();
        }

        return View();
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginUserViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var res = await _userInterface.LoginAsync(viewModel);
            if (res.IsSuccess)
            {
                var claims = new List<Claim>
                {
                    new Claim("email", viewModel.Email)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true, // Cookie persists between browser sessions
                    ExpiresUtc = DateTimeOffset.Now.AddHours(1), // Set the expiration time as needed.
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToAction("index", "home");
            }

            if (res.user.Email != "")
            {
                ModelState.AddModelError("Email", res.ErrorMessage);
            }
            else
            {
                ModelState.AddModelError("Password", res.ErrorMessage);
            }
        }

        return View();
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("index", "home");
    }
}
