using System.Threading.Tasks;
using Identity.Server.MVC.Services.Abstractions;
using Microsoft.Extensions.Logging;

namespace Identity.Server.MVC.Services.Mock;

public class MockSmsService : ISmsService
{
    private readonly ILogger<MockSmsService> _logger;
    
    public MockSmsService(ILogger<MockSmsService> logger)
    {
        _logger = logger;
    }
    
    public Task SendSmsAsync(string phoneNumber, string message)
    {
        var sanitizedPhoneNumber = phoneNumber.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
        var sanitizedMessage = message.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
        _logger.LogInformation($"Sending SMS to {sanitizedPhoneNumber} with message: {sanitizedMessage}");
        return Task.CompletedTask;
    }
}