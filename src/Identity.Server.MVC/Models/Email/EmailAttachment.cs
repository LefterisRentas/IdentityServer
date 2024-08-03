namespace Identity.Server.MVC.Models.Email;

public class EmailAttachment
{
    public string FileName { get; set; }
    public byte[] FileBytes { get; set; }
    public string MimeType { get; set; }
}