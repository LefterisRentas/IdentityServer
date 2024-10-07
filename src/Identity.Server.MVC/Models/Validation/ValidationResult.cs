namespace Identity.Server.MVC.Models.Validation;

public class ValidationResult
{
    public bool IsValid { get; set; }
    public string? ErrorMessage { get; set; }
}