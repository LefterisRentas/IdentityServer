namespace Identity.Server.MVC.Options;

public class SmtpEmailSettings
{
    public string Server { get; set; }
    public ushort Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string DefaultSender { get; set; }
    public string DefaultSenderName { get; set; }
    public bool UseSsl { get; set; }
    public bool UseCredentials { get; set; }
}