namespace Identity.Server.Extended.Constants.EmailCodes;

/// <summary>
/// Email success codes.
/// </summary>
public class EmailSuccessCode : EmailCode
{
    /// <summary>
    /// Email sent successfully.
    /// </summary>
    public static EmailCode Success { get; } = new EmailSuccessCode("Success", "Email sent successfully.");
    
    /// <summary>
    /// Protected constructor for <see cref="EmailSuccessCode"/>.
    /// </summary>
    /// <param name="code"></param>
    /// <param name="description"></param>
    protected EmailSuccessCode(string code, string description) : base(code, description)
    {
    }
}