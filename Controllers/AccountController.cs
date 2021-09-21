using EmployeeManagementAuth.Models;
using EmployeeManagementAuth.Models.ViewModels;
using EmployeeManagementAuth.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementAuth.Controllers
{
    public class AccountController : Controller
    {
        #region Fields
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;

        private readonly string MAILJET_CLIENT_EMAIL_ADDRESS = Environment.GetEnvironmentVariable("MAILJET_CLIENT_EMAIL_ADDRESS");
        #endregion

        #region Ctor
        public AccountController(
                                    ApplicationDbContext db,
                                    UserManager<ApplicationUser> userManager,
                                    SignInManager<ApplicationUser> signInManager,
                                    RoleManager<IdentityRole> roleManager,
                                    IEmailSender emailSender)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
        }
        #endregion

        #region Login
        //  GET-Login
        public IActionResult Login()
        {
            return View();
        }

        //  POST-Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Invalid login attempt");
            }
            return View(model);
        }
        #endregion

        #region Register
        //  GET-Register
        public IActionResult Register()
        {
            return View();
        }
       
        //  POST-Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Name = model.Name
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.RoleName);
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    await _emailSender.SendEmailAsync(user.Email, "Account Created", $"You have successfully registered to EmployeeManagement application with {user.Email} address.");
                    await _emailSender.SendEmailAsync(MAILJET_CLIENT_EMAIL_ADDRESS, "New Account Registered", $"Account Details:<br /><br />Id: {user.Id}<br />Name: {user.Name}<br />Email: {user.Email}");

                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        #endregion

        #region LogOff
        //  POST-LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        #endregion
    }
}
