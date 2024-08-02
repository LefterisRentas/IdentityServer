using Identity.Server.MVC.Constants;
using Microsoft.AspNetCore.Identity;

namespace Identity.Server.MVC.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    [PersonalData]
    public string[] ExternalProviderIds { get; set; }
    public TwoFactorProviders TwoFactorProvider { get; set; }
}