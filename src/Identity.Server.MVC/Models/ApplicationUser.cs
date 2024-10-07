using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Identity.Server.Extended.Constants;
using Identity.Server.MVC.Data.User;
using Microsoft.AspNetCore.Identity;

namespace Identity.Server.MVC.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    [PersonalData]
    public string[] ExternalProviderIds { get; set; } = [];
    public TwoFactorProviders TwoFactorProvider { get; set; } = TwoFactorProviders.None;
    //TODO: Is this necessary or is it just for seeding? Needs more investigating around the Microsoft.AspNetCore.Identity package
    [PersonalData]
    public List<IdentityUserClaim<string>> Claims { get; set; } = new();
    [ForeignKey("ProfilePicture")]
    public int? ProfilePictureId { get; set; }
    public ProfilePicture? ProfilePicture { get; set; } = new();
}