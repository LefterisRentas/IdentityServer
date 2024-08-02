using System;
using System.Threading.Tasks;
using Identity.Server.MVC.Constants.EmailCodes;
using Identity.Server.MVC.Models.Email;
using Identity.Server.MVC.Services.Abstractions;
using Microsoft.Extensions.Logging;

namespace Identity.Server.MVC.Services.Mock;

public class MockEmailService : IEmailService
{
    private readonly ILogger<MockEmailService> _logger;
    
    public MockEmailService(ILogger<MockEmailService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public Task<EmailCode> SendEmailAsync(string[] recipients, string? senderName, string? senderAddress, string subject, string body, string[]? bccRecipients = null, EmailAttachment[] attachments = null)
    {
        _logger.LogInformation("Email sent to {Recipients} from {Sender} with subject {Subject} and message {Message}", recipients, senderName + senderAddress, subject, recipients);
        return Task.FromResult(EmailSuccessCode.Success);
    }
}