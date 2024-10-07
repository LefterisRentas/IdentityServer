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
        _logger.LogInformation($"Sending SMS to {phoneNumber} with message: {message}");
        return Task.CompletedTask;
    }
}