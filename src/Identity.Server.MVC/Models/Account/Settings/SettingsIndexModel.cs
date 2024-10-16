﻿using System.ComponentModel.DataAnnotations;
using Identity.Server.Extended.Constants;
using Microsoft.AspNetCore.Http;

namespace Identity.Server.MVC.Models.Account.Settings;

public class SettingsViewModel
{
    [Required]
    public string? Username { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Phone]
    public string? PhoneNumber { get; set; }

    public bool TwoFactorEnabled { get; set; }
    public TwoFactorProviders TwoFactorProvider { get; set; } = TwoFactorProviders.None;
    public IFormFile? ProfilePicture { get; set; }
}