using System.Threading.Tasks;
using Identity.Server.MVC.Models;
using Identity.Server.MVC.Models.Account.Settings;
using Identity.Server.MVC.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Server.MVC.Controllers.Account
{
    [Authorize]
    [Route("account/settings")]
    public class SettingsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly ISmsService _smsService;

        public SettingsController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailService emailService, ISmsService smsService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _smsService = smsService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var model = new SettingsViewModel
            {
                Username = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                TwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user),
                TwoFactorProvider = user.TwoFactorProvider
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(SettingsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (user.Email != model.Email)
            {
                var token = await _userManager.GenerateChangeEmailTokenAsync(user, model.Email);
                await _emailService.SendEmailAsync(new[] { model.Email }, null, null, "Confirm your email", $"Your confirmation code is {token}");
                TempData["NewEmail"] = model.Email;
                return RedirectToAction("ConfirmEmailChange");
            }

            if (user.PhoneNumber != model.PhoneNumber)
            {
                var token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, model.PhoneNumber);
                await _smsService.SendSmsAsync(model.PhoneNumber, $"Your confirmation code is {token}");
                TempData["NewPhoneNumber"] = model.PhoneNumber;
                return RedirectToAction("ConfirmPhoneNumberChange");
            }

            if (model.TwoFactorEnabled)
            {
                if (!await _userManager.GetTwoFactorEnabledAsync(user))
                {
                    await _userManager.SetTwoFactorEnabledAsync(user, true);
                }
                user.TwoFactorProvider = model.TwoFactorProvider;
            }
            else
            {
                await _userManager.SetTwoFactorEnabledAsync(user, false);
            }

            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);

            return RedirectToAction("Index", new { Message = "Settings updated successfully" });
        }

        [HttpGet]
        [Route("confirm-email-change")]
        public IActionResult ConfirmEmailChange()
        {
            var newEmail = TempData["NewEmail"]?.ToString();
            if (string.IsNullOrEmpty(newEmail))
            {
                return RedirectToAction("Index");
            }

            var model = new EmailChangeViewModel { NewEmail = newEmail };
            return View(model);
        }

        [HttpPost]
        [Route("confirm-email-change")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmEmailChange(EmailChangeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var result = await _userManager.ChangeEmailAsync(user, model.NewEmail, model.Token);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                return RedirectToAction("Index", new { Message = "Email updated successfully" });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpGet]
        [Route("confirm-phone-number-change")]
        public IActionResult ConfirmPhoneNumberChange()
        {
            var newPhoneNumber = TempData["NewPhoneNumber"]?.ToString();
            if (string.IsNullOrEmpty(newPhoneNumber))
            {
                return RedirectToAction("Index");
            }

            var model = new PhoneNumberChangeViewModel { NewPhoneNumber = newPhoneNumber };
            return View(model);
        }

        [HttpPost]
        [Route("confirm-phone-number-change")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmPhoneNumberChange(PhoneNumberChangeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var result = await _userManager.ChangePhoneNumberAsync(user, model.NewPhoneNumber, model.Token);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                return RedirectToAction("Index", new { Message = "Phone number updated successfully" });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
    }
}
