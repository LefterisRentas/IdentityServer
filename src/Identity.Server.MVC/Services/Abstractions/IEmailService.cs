using System.Threading.Tasks;
using Identity.Server.Extended.Constants.EmailCodes;
using Identity.Server.MVC.Models.Email;

namespace Identity.Server.MVC.Services.Abstractions;

public interface IEmailService
{
    Task<EmailCode> SendEmailAsync(string[] recipients, string? senderName, string? senderAddress, string subject, string body, string[]? bccRecipients = null, EmailAttachment[] attachments = null);
}