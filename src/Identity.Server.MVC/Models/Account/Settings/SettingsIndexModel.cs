using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Identity.Server.Extended.Constants;
using IdentityModel;
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
    public Claim FirstName = new(JwtClaimTypes.GivenName, string.Empty);
    public Claim LastName = new(JwtClaimTypes.FamilyName, string.Empty);
    // Computed properties to show and update the values
    public string FirstNameValue
    {
        get => FirstName?.Value ?? string.Empty;
        set
        {
            if (FirstName != null)
            {
                FirstName = new Claim(FirstName.Type, value); // Update claim value
            }
        }
    }

    public string LastNameValue
    {
        get => LastName?.Value ?? string.Empty;
        set
        {
            if (LastName != null)
            {
                LastName = new Claim(LastName.Type, value); // Update claim value
            }
        }
    }
}