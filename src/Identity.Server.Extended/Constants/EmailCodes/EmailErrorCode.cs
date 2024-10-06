namespace Identity.Server.Extended.Constants.EmailCodes;
/// <summary>
/// <inheritdoc cref="EmailCode"/>
/// </summary>
public class EmailErrorCode : EmailCode
{
    /// <summary>
    /// The email is invalid.
    /// </summary>
    public static EmailCode InvalidEmail = new EmailErrorCode("InvalidEmail", "The email is invalid.");
    /// <summary>
    /// The recipient is invalid.
    /// </summary>
    public static EmailCode InvalidRecipient = new EmailErrorCode("InvalidRecipient", "The recipient is invalid.");
    /// <summary>
    /// The sender is invalid.
    /// </summary>
    public static EmailCode InvalidSender = new EmailErrorCode("InvalidSender", "The sender is invalid.");
    /// <summary>
    /// The subject is invalid.
    /// </summary>
    public static EmailCode InvalidSubject = new EmailErrorCode("InvalidSubject", "The subject is invalid.");
    /// <summary>
    /// The body is invalid.
    /// </summary>
    public static EmailCode InvalidBody = new EmailErrorCode("InvalidBody", "The body is invalid.");
    /// <summary>
    /// The attachment is invalid.
    /// </summary>
    public static EmailCode InvalidAttachment = new EmailErrorCode("InvalidAttachment", "The attachment is invalid.");
    /// <summary>
    /// The template is invalid.
    /// </summary>
    public static EmailCode InvalidTemplate = new EmailErrorCode("InvalidTemplate", "The template is invalid.");
    /// <summary>
    /// The configuration is invalid.
    /// </summary>
    public static EmailCode InvalidConfiguration = new EmailErrorCode("InvalidConfiguration", "The configuration is invalid.");
    /// <summary>
    /// The credentials are invalid.
    /// </summary>
    public static EmailCode InvalidCredentials = new EmailErrorCode("InvalidCredentials", "The credentials are invalid.");
    /// <summary>
    /// The token is invalid.
    /// </summary>
    public static EmailCode InvalidToken = new EmailErrorCode("InvalidToken", "The token is invalid.");
    /// <summary>
    /// The code is invalid.
    /// </summary>
    public static EmailCode InvalidCode = new EmailErrorCode("InvalidCode", "The code is invalid.");
    /// <summary>
    /// The email is already verified.
    /// </summary>
    public static EmailCode SendEmailFailed = new EmailErrorCode("SendEmailFailed", "Failed to send email.");

    /// <summary>
    /// <inheritdoc cref="EmailCode"/>
    /// </summary>
    private EmailErrorCode(string code, string description) : base(code, description)
    {
    }
}