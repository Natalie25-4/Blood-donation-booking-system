using BloodDonation.Web.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BloodDonation.Web.Models;

namespace BloodDonation.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationViewModel model)
       {
         //check the required registration fields before creating the account
         if (!ModelState.IsValid)
       {
        return View(model);
       }

        // Only the account (email, password, Donor role)is created here.
        // Storing the donor details and questionnaire answers is part of the registration split (Part 4).
        if (model.Password != model.ConfirmPassword)
       {
          ViewData["RegisterErrors"] = new[] { "Passwords do not match." };
          return View(model);
       }

        var user = new IdentityUser
     {
        UserName = model.Email,
        Email = model.Email
    };

       var result = await _userManager.CreateAsync(user, model.Password);

       if (!result.Succeeded)
       {
        ViewData["RegisterErrors"] = result.Errors
            .Select(e => e.Description)
            .ToArray();

        return View(model);
      }

        await _userManager.AddToRoleAsync(user, Roles.Donor);
        await _signInManager.SignInAsync(user, isPersistent: false);

        return RedirectToAction("Dashboard", "Donor");
}

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> Login(string Email, string Password, bool RememberMe, string? returnUrl = null)
        {
            return SignInAndRedirect(Email, Password, RememberMe, returnUrl);
        }

        [HttpGet]
        public IActionResult StaffLogin(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // Staff and coordinators both sign in here.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> StaffLogin(string Email, string Password, string? returnUrl = null)
        {
            return SignInAndRedirect(Email, Password, rememberMe: false, returnUrl);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private async Task<IActionResult> SignInAndRedirect(string email, string password, bool rememberMe, string? returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["Email"] = email;

            var result = await _signInManager.PasswordSignInAsync(email ?? "", password ?? "", rememberMe, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                ViewData["LoginError"] = "Invalid email or password.";
                return View();
            }

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            var user = await _userManager.FindByEmailAsync(email!);
            var roles = user is null ? new List<string>() : await _userManager.GetRolesAsync(user);

            if (roles.Contains(Roles.Coordinator))
            {
                return RedirectToAction("Dashboard", "Coordinator");
            }

            if (roles.Contains(Roles.Staff))
            {
                return RedirectToAction("Dashboard", "Staff");
            }

            return RedirectToAction("Dashboard", "Donor");
        }
    }
}
