namespace Identity.Server.MVC.Constants.EmailCodes;

public abstract class EmailCode
{
    public string Code { get; private set; }
    public string Description { get; private set; }
    
    public bool IsSuccess => this is EmailSuccessCode;
    
    protected EmailCode(string code, string description)
    {
        Code = code;
        Description = description;
    }
}