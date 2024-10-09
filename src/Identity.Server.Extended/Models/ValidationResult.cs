namespace Identity.Server.Extended.Models;

/// <summary>
/// Validation result class for returning results of validation.
/// </summary>
public class ValidationResult
{
    /// <summary>
    /// If the client is valid.
    /// </summary>
    public bool IsValid { get; private set; }

    /// <summary>
    /// The validation errors.
    /// </summary>
    public string[] ValidationErrors { get; private set; }

    public ValidationResult(bool isValid, string[] validationErrors)
    {
        IsValid = isValid;
        ValidationErrors = validationErrors;
    }
}