using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Identity.Server.MVC.Constants.EmailCodes;
using Identity.Server.MVC.Constants.ErrorCodes;
using Identity.Server.MVC.Models.Email;
using Identity.Server.MVC.Options;
using Identity.Server.MVC.Services.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;

namespace Identity.Server.MVC.Services;

public class SmtpEmailService : IEmailService
{
    private SmtpEmailSettings _smtpEmailSettings;
    private readonly ILogger<SmtpEmailService> _logger;

    public SmtpEmailService(IOptionsMonitor<SmtpEmailSettings> smtpEmailSettings, ILogger<SmtpEmailService> logger)
    {
        _smtpEmailSettings = smtpEmailSettings.CurrentValue ?? throw new ArgumentNullException(nameof(smtpEmailSettings));
        smtpEmailSettings.OnChange(settings => _smtpEmailSettings = settings ?? _smtpEmailSettings);
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<EmailCode> SendEmailAsync(string[] recipients, string? senderName, string? senderAddress, string subject, string body, string[]? bccRecipients = null, EmailAttachment[] attachments = null)
    {
        var message = new MimeMessage();

        // Set sender
        if (senderName != null && senderAddress != null)
        {
            message.From.Add(new MailboxAddress(senderName, senderAddress));
        }
        else
        {
            message.From.Add(new MailboxAddress(_smtpEmailSettings.DefaultSenderName, _smtpEmailSettings.DefaultSender));
        }

        if (message.From.Count == 0)
        {
            return EmailErrorCode.InvalidSender;
        }

        // Set recipients
        try
        {
            message.To.AddRange(recipients.Select(InternetAddress.Parse));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add recipients to the email message.");
            return EmailErrorCode.InvalidRecipient;
        }

        // Set BCC recipients
        if (bccRecipients?.Length > 0)
        {
            try
            {
                message.Bcc.AddRange(bccRecipients.Select(InternetAddress.Parse));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add BCC recipients to the email message.");
                return EmailErrorCode.InvalidRecipient;
            }
        }

        // Set subject and body
        message.Subject = subject;
        var bodyPart = new TextPart(TextFormat.Html)
        {
            Text = body
        };

        // Handle attachments
        if (attachments?.Length > 0)
        {
            var multipart = new Multipart("mixed")
            {
                bodyPart
            };

            foreach (var attachment in attachments)
            {
                var contentType = attachment.MimeType;
                var contentTypeParts = contentType.Split('/');
                var attachmentPart = new MimePart(contentTypeParts[0], contentTypeParts[1])
                {
                    Content = new MimeContent(new MemoryStream(attachment.FileBytes)),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = attachment.FileName
                };
                multipart.Add(attachmentPart);
            }

            message.Body = multipart;
        }
        else
        {
            message.Body = bodyPart;
        }

        // Send email
        try
        {
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smtpEmailSettings.Server, _smtpEmailSettings.Port, _smtpEmailSettings.UseSsl);
                if(_smtpEmailSettings.UseCredentials)
                {
                    await client.AuthenticateAsync(_smtpEmailSettings.Username, _smtpEmailSettings.Password);
                }
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            return EmailSuccessCode.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email.");
            return EmailErrorCode.SendEmailFailed;
        }
    }
}