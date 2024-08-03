using System.ComponentModel.DataAnnotations;

namespace Identity.Server.MVC.Views.TwoFactor;

public class TwoFactorViewModel
{
    [Required]
    [Display(Name = "Code")]
    public string Code { get; set; }

    [Display(Name = "Remember this browser")]
    public bool RememberBrowser { get; set; }

    [Display(Name = "Remember me")]
    public bool RememberMe { get; set; }
    
    public string ReturnUrl { get; set; }
}