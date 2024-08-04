using System.ComponentModel.DataAnnotations;

namespace Identity.Server.MVC.Models.Account.Settings;

public class EmailChangeViewModel
{
    [Required]
    [EmailAddress]
    [Display(Name = "New Email")]
    public string NewEmail { get; set; }

    [Required]
    public string Token { get; set; }
}