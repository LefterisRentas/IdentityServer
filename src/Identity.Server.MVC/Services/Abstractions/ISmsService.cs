using System.Threading.Tasks;

namespace Identity.Server.MVC.Services.Abstractions;

public interface ISmsService
{
    Task SendSmsAsync(string phoneNumber, string message);
}