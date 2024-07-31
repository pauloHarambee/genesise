using GenesisExchange.Controllers.sub_controllers;
using GenesisExchange.Data;
using GenesisExchange.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;

namespace GenesisExchange.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager,
            SignInManager<AppUser> signInManager, AppDbContext appDbContext) : base(appDbContext, userManager, roleManager, signInManager)
        {
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string ReturnUrl, string Message)
        {
            if (!string.IsNullOrEmpty(Message))
                ViewBag.Message = Message;

            return View(new LoginModel
            {
                ReturnUrl = ReturnUrl
            });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please verify your credentials");
                return View(model);
            }
            AppUser user = await _userManager
                .FindByNameAsync(model.UserName);
            if (user != null)
            {
                var identityResult =
                await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                if (identityResult.Succeeded)
                {
                    string redirectByRole = string.Empty;

                    if (await _userManager.IsInRoleAsync(user, "Sender"))
                        redirectByRole = "/dashboard/index";
                    if (await _userManager.IsInRoleAsync(user, "Reciever"))
                        redirectByRole = "/dashboard/recieve";

                    if (string.IsNullOrEmpty(redirectByRole))
                    {
                        ModelState.AddModelError("", "User role for this role is not recognised!");
                        return View(model);
                    }
                    return Redirect(model.ReturnUrl ?? redirectByRole);
                }
            }
            ModelState.AddModelError("", "Invalid username or password!");
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
        private void AddIdentityErrors(IEnumerable<IdentityError> Errors)
        {
            foreach (var error in Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
        }
        private async void RollBackIdentityUser(AppUser user) => await _userManager.DeleteAsync(user);

    }
}
