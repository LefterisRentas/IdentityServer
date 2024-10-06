using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Identity.Server.MVC.Services.Abstractions;
using System.Threading.Tasks;
using Identity.Server.Extended.Constants;
using Identity.Server.MVC.Models;
using Identity.Server.MVC.Models.Account;
using Identity.Server.MVC.Views.TwoFactor;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace Identity.Server.MVC.Controllers.Account
{
    [Route("two-factor")]
    public class TwoFactorController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly ISmsService _smsService;
        private readonly ILogger<TwoFactorController> _logger;

        public TwoFactorController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailService emailService,
            ISmsService smsService,
            ILogger<TwoFactorController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _smsService = smsService;
            _logger = logger;
        }

        // GET
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await HttpContext.AuthenticateAsync(IdentityConstants.TwoFactorUserIdScheme);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var code = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
            if (user.TwoFactorProvider == TwoFactorProviders.Email)
            {
                if (string.IsNullOrWhiteSpace(user.Email))
                {
                    _logger.LogWarning("User {UserName} does not have an email address.", user.UserName);
                    return View();
                }
                await _emailService.SendEmailAsync(
                    [user.Email],
                    null,
                    null,
                    "Two-Factor Authentication Code", $"Your authentication code is {code}");
            }
            else if (user.TwoFactorProvider == TwoFactorProviders.Phone)
            {
                code = await _userManager.GenerateTwoFactorTokenAsync(user, "Phone");
                await _smsService.SendSmsAsync(user.PhoneNumber, $"Your authentication code is {code}");
            }
            return View();
        }

        // POST
        [HttpPost]
        [Route("verify-code")]
        public async Task<IActionResult> VerifyCode(TwoFactorViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            var authenticateResult = await HttpContext.AuthenticateAsync(IdentityConstants.TwoFactorUserIdScheme);
            if (!authenticateResult.Succeeded)
            {
                await HttpContext.SignOutAsync(IdentityConstants.TwoFactorUserIdScheme);
                return RedirectToAction("Index", "Home");
            }

            var userId = authenticateResult.Principal.FindFirstValue(JwtClaimTypes.Subject);
            if (userId == null)
            {
                await HttpContext.SignOutAsync(IdentityConstants.TwoFactorUserIdScheme);
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                await HttpContext.SignOutAsync(IdentityConstants.TwoFactorUserIdScheme);
                return RedirectToAction("Index", "Home");
            }

            var provider = user.TwoFactorProvider == TwoFactorProviders.Phone ? "Phone" : "Email";
            _logger.LogInformation($"Provider: {provider}, Code: {model.Code}, User: {user.UserName}");

            // Verify the code
            var result = await _signInManager.TwoFactorSignInAsync(provider, model.Code, model.RememberMe, model.RememberBrowser);
            if (result.Succeeded)
            {
                _logger.LogInformation("Two-factor authentication succeeded for user {UserName}.", user.UserName);
                return RedirectToAction("Index", "Home");
            }

            if (result.IsLockedOut)
            {
                await HttpContext.SignOutAsync(IdentityConstants.TwoFactorUserIdScheme);
                //TODO: Get the lockout end date
                return View("Lockout", new LockoutViewModel{ LockoutEnd = DateTimeOffset.MaxValue, IsPermanentLockout = result.IsNotAllowed });
            }

            _logger.LogWarning("Invalid code for user {UserName}", user.UserName);
            ModelState.AddModelError(string.Empty, "Invalid code.");
            return View("Index", model);
        }

        // POST
        [Route("resend-code")]
        [HttpPost]
        public async Task<IActionResult> ResendCode()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(IdentityConstants.TwoFactorUserIdScheme);
            if (!authenticateResult.Succeeded)
            {
                _logger.LogWarning("Unable to authenticate two-factor user.");
                return RedirectToAction("Index", "Home");
            }

            var userId = authenticateResult.Principal.FindFirstValue(JwtClaimTypes.Subject);
            if (userId == null)
            {
                _logger.LogWarning("Unable to find user ID in authentication context.");
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("Unable to load two-factor authentication user.");
                return RedirectToAction("Index", "Home");
            }

            var provider = user.TwoFactorProvider == TwoFactorProviders.Phone ? "Phone" : "Email";
            var code = await _userManager.GenerateTwoFactorTokenAsync(user, provider);
            _logger.LogInformation($"Generated code: {code} for user: {user.UserName} via {provider}");
            if (user.TwoFactorProvider == TwoFactorProviders.Phone)
            {
                await _smsService.SendSmsAsync(user.PhoneNumber, $"Your authentication code is {code}");
            }
            else
            {
                if(string.IsNullOrWhiteSpace(user.Email))
                {
                    _logger.LogWarning("User {UserName} does not have an email address.", user.UserName);
                    return RedirectToAction("Index");
                }
                await _emailService.SendEmailAsync([user.Email], null, null, "Two-Factor Authentication Code", $"Your authentication code is {code}");
            }

            _logger.LogInformation("Two-factor authentication code resent to user {UserName} via {Provider}.", user.UserName, provider);
            return RedirectToAction("Index");
        }
    }
}
