namespace Identity.Server.MVC.Constants.EmailCodes;

public class EmailSuccessCode : EmailCode
{
    public static EmailCode Success { get; } = new EmailSuccessCode("Success", "Email sent successfully.");
    
    protected EmailSuccessCode(string code, string description) : base(code, description)
    {
    }
}