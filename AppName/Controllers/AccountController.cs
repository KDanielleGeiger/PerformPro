using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PerformPro.Models;
using Microsoft.AspNetCore.Authorization;

namespace PerformPro.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<AppUser> UserManager { get; }

        private SignInManager<AppUser> SignInManager { get; }

        private RoleManager<AppRole> RoleManager { get; }

        public AccountController(UserManager<AppUser> UserManager, SignInManager<AppUser> SignInManager, RoleManager<AppRole> RoleManager)
        {
            this.UserManager = UserManager;
            this.SignInManager = SignInManager;
            this.RoleManager = RoleManager;
        }

        public async Task<IActionResult> Index()
        {
            var UserID = await UserManager.GetUserAsync(HttpContext.User);
            AccountModel accModel = new AccountModel(UserID.Email);
            return View("Index", accModel);
        }

        [AllowAnonymous]
        public IActionResult ToLogin()
        {
            return View("Login");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginInfo)
        {
            if ((loginInfo.Email == null) || (loginInfo.Password == null))
            {
                ModelState.AddModelError(string.Empty, $"Email or Password incorrect.");
                return View("Login", loginInfo);
            }

            var result = await SignInManager.PasswordSignInAsync(loginInfo.Email, loginInfo.Password, false, false);
            if (result.Succeeded)
            {
                AppUser user = await UserManager.FindByEmailAsync(loginInfo.Email);
                if (user.Deleted == true)
                {
                    await SignInManager.SignOutAsync();
                    return View("LoginFailure");
                }
                else if (user.PasswordChanged == false)
                {
                    PasswordModel pwModel = new PasswordModel(loginInfo.Password, "", "");
                    return View("ChangePasswordPrompt", pwModel);
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, $"Email or Password incorrect.");
                return View("Login", loginInfo);
            }

            //Created roles using this
            /*var role1 = new AppRole();
            role1.Name = "Administrator";
            await RoleManager.CreateAsync(role1);

            var role2 = new AppRole();
            role2.Name = "Supervisor";
            await RoleManager.CreateAsync(role2);*/
        }

        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        
        public IActionResult ToChangePassword()
        {
            return View("ChangePassword");
        }

        public async Task<IActionResult> ChangePassword(PasswordModel pwModel)
        {
            if ((pwModel.OldPassword == null) || (pwModel.NewPassword == null) || (pwModel.NewPasswordConfirm == null))
            {
                ModelState.AddModelError(string.Empty, $"One or more fields were left empty.");
                return View("ChangePassword", pwModel);
            }

            AppUser user = await UserManager.GetUserAsync(HttpContext.User);
            var result = await UserManager.CheckPasswordAsync(user, pwModel.OldPassword);

            if (result == false)
            {
                ModelState.AddModelError(string.Empty, $"Password is incorrect.");
                return View("ChangePassword", pwModel);
            }
            else if (!(pwModel.NewPassword == pwModel.NewPasswordConfirm))
            {
                ModelState.AddModelError(string.Empty, $"Passwords do not match.");
                return View("ChangePassword", pwModel);
            }
            else if (pwModel.NewPassword.Length < 6)
            {
                ModelState.AddModelError(string.Empty, $"Passwords must at least 6 characters.");
                return View("ChangePassword", pwModel);
            }

            var token = await UserManager.GeneratePasswordResetTokenAsync(user);
            await UserManager.ResetPasswordAsync(user, token, pwModel.NewPassword);
            user.PasswordChanged = true;
            await UserManager.UpdateAsync(user);
            return RedirectToAction("Index", "Account");
        }

        public async Task<IActionResult> ChangePasswordFromPrompt(PasswordModel pwModel)
        {
            if ((pwModel.OldPassword == "") || (pwModel.NewPassword == "") || (pwModel.NewPasswordConfirm == ""))
            {
                ModelState.AddModelError(string.Empty, $"One or more fields were left empty.");
                return View("ChangePasswordPrompt", pwModel);
            }

            AppUser user = await UserManager.GetUserAsync(HttpContext.User);
            var result = await UserManager.CheckPasswordAsync(user, pwModel.OldPassword);
            
            if (result == false)
            {
                ModelState.AddModelError(string.Empty, $"Password is incorrect.");
                return View("ChangePasswordPrompt", pwModel);
            }
            else if (!(pwModel.NewPassword == pwModel.NewPasswordConfirm))
            {
                ModelState.AddModelError(string.Empty, $"Passwords do not match.");
                return View("ChangePasswordPrompt", pwModel);
            }
            else if (pwModel.NewPassword.Length < 6)
            {
                ModelState.AddModelError(string.Empty, $"Passwords must at least 6 characters.");
                return View("ChangePasswordPrompt", pwModel);
            }

            var token = await UserManager.GeneratePasswordResetTokenAsync(user);
            await UserManager.ResetPasswordAsync(user, token, pwModel.NewPassword);
            user.PasswordChanged = true;
            await UserManager.UpdateAsync(user);
            return RedirectToAction("Index", "Home");
        }

        /*public async Task<IActionResult> Register()
        {
            try
            {
                ViewBag.Message = "User Already Registered";

                AppUser user = await UserManager.FindByNameAsync("TestUser");
                if (user == null)
                {
                    user = new AppUser();
                    user.UserName = "TestUser";
                    user.Email = "TestUser@example.com";
                    user.AccountDeleted = false;
                    user.PasswordChanged = true;

                    IdentityResult result = await UserManager.CreateAsync(user, "Testing123!");
                    ViewBag.Message = "User was created";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }

            return View();
        }*/
    }
}