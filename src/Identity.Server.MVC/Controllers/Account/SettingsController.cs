using System;
using System.IO;
using System.Net.Mime;
using System.Threading.Tasks;
using Identity.Server.MVC.Data.User;
using Identity.Server.MVC.Models;
using Identity.Server.MVC.Models.Account.Settings;
using Identity.Server.MVC.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Identity.Server.MVC.Controllers.Account
{
    [Authorize]
    [Route("account/settings")]
    public class SettingsController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IEmailService emailService,
        ISmsService smsService,
        ILogger<SettingsController> logger,
        IProfilePictureService profilePictureService)
        : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        private readonly IEmailService _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        private readonly ISmsService _smsService = smsService ?? throw new ArgumentNullException(nameof(smsService));
        private readonly ILogger<SettingsController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IProfilePictureService _profilePictureService = profilePictureService ?? throw new ArgumentNullException(nameof(profilePictureService));

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            FormFile? profilePicture;
            using (var stream = new MemoryStream(user.ProfilePicture?.Data ?? new byte[0]))
            {
                profilePicture = new FormFile(stream, 0, stream.Length, user.ProfilePicture?.FileName ?? "profilePicture", user.ProfilePicture?.ContentType ?? MediaTypeNames.Image.Jpeg);
            }
            var model = new SettingsViewModel
            {
                Username = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                PhoneNumber = user?.PhoneNumber ?? string.Empty,
                TwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user ?? throw new InvalidOperationException("User not found.")),
                TwoFactorProvider = user.TwoFactorProvider,
                ProfilePicture = profilePicture
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
                _logger.LogError("User not found.");
                return NotFound("User not found.");
            }

            if (model.ProfilePicture != null)
            {
                using (var stream = new MemoryStream())
                {
                    if(user.ProfilePicture is not null)
                    {
                        await _profilePictureService.DeleteProfilePictureAsync(user.ProfilePicture.Id);
                    }
                    await model.ProfilePicture.CopyToAsync(stream);
                    var profilePictureId = (await _profilePictureService.SaveProfilePictureAsync(new ProfilePicture
                    {
                        Data = stream.ToArray(),
                        FileName = model.ProfilePicture.FileName,
                        ContentType = model.ProfilePicture.ContentType
                    }))?.Id;
                    if(profilePictureId is null) throw new ArgumentNullException($"Failed to save profile picture, {profilePictureId}", nameof(profilePictureId));
                    user.ProfilePicture = await _profilePictureService.GetProfilePictureAsync(profilePictureId.Value);
                }
            }

            if (user.Email != model.Email)
            {
                var token = await _userManager.GenerateChangeEmailTokenAsync(user, model.Email ?? throw new InvalidOperationException("Email is required."));
                await _emailService.SendEmailAsync(new[] { model.Email }, null, null, "Confirm your email", $"Your confirmation code is {token}");
                TempData["NewEmail"] = model.Email;
                return RedirectToAction("ConfirmEmailChange");
            }

            if (user.PhoneNumber != model.PhoneNumber && !string.IsNullOrEmpty(model.PhoneNumber))
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