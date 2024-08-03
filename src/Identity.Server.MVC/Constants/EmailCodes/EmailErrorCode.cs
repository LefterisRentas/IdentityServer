using Identity.Server.MVC.Constants.EmailCodes;

namespace Identity.Server.MVC.Constants.ErrorCodes;

public class EmailErrorCode : EmailCode
{
    public static EmailCode InvalidEmail = new EmailErrorCode("InvalidEmail", "The email is invalid.");
    public static EmailCode InvalidRecipient = new EmailErrorCode("InvalidRecipient", "The recipient is invalid.");
    public static EmailCode InvalidSender = new EmailErrorCode("InvalidSender", "The sender is invalid.");
    public static EmailCode InvalidSubject = new EmailErrorCode("InvalidSubject", "The subject is invalid.");
    public static EmailCode InvalidBody = new EmailErrorCode("InvalidBody", "The body is invalid.");
    public static EmailCode InvalidAttachment = new EmailErrorCode("InvalidAttachment", "The attachment is invalid.");
    public static EmailCode InvalidTemplate = new EmailErrorCode("InvalidTemplate", "The template is invalid.");
    public static EmailCode InvalidConfiguration = new EmailErrorCode("InvalidConfiguration", "The configuration is invalid.");
    public static EmailCode InvalidCredentials = new EmailErrorCode("InvalidCredentials", "The credentials are invalid.");
    public static EmailCode InvalidToken = new EmailErrorCode("InvalidToken", "The token is invalid.");
    public static EmailCode InvalidCode = new EmailErrorCode("InvalidCode", "The code is invalid.");
    public static EmailCode SendEmailFailed = new EmailErrorCode("SendEmailFailed", "Failed to send email.");

    protected EmailErrorCode(string code, string description) : base(code, description)
    {
    }
}