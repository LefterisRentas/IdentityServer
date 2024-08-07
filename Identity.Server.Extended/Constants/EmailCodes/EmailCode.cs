namespace Identity.Server.Extended.Constants.EmailCodes;

/// <summary>
/// The code returned when an email is successfully sent or failed to send.
/// </summary>
public abstract class EmailCode
{
    /// <summary>
    /// The success or failure code.
    /// </summary>
    public string Code { get; private set; }
    /// <summary>
    /// The success or failure description.
    /// </summary>
    public string Description { get; private set; }
    
    /// <summary>
    /// Indicates if the email code is a success code.
    /// </summary>
    public bool IsSuccess => this is EmailSuccessCode;
    
    /// <summary>
    /// A protected constructor for the EmailCode class.
    /// </summary>
    /// <param name="code"></param>
    /// <param name="description"></param>
    protected EmailCode(string code, string description)
    {
        Code = code;
        Description = description;
    }
}