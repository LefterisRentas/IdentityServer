using System.ComponentModel.DataAnnotations;

namespace Identity.Server.MVC.Models.Account.Settings;

public class PhoneNumberChangeViewModel
{
    [Required]
    [Phone]
    [Display(Name = "New Phone Number")]
    public string NewPhoneNumber { get; set; }

    [Required]
    public string Token { get; set; }
}