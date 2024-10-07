using System;

namespace Identity.Server.MVC.Models.Email;

public class EmailAttachment
{
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    public string FileName { get; set; } = string.Empty;
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    public byte[] FileBytes { get; set; } = Array.Empty<byte>();
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    public string MimeType { get; set; } = string.Empty;
}