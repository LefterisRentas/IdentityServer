using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Identity.Server.Extended.Constants;
using Microsoft.AspNetCore.Identity;

namespace Identity.Server.MVC.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    [PersonalData]
    public string[] ExternalProviderIds { get; set; }
    public TwoFactorProviders TwoFactorProvider { get; set; }
    //TODO: Is this necessary or is it just for seeding? Needs more investigating around the Microsoft.AspNetCore.Identity package
    [PersonalData]
    public List<IdentityUserClaim<string>> Claims { get; set; }
}