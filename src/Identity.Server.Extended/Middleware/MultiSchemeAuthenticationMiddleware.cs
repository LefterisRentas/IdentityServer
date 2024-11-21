using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Server.Extended.Middleware;

public class MultiSchemeAuthenticationMiddleware
{
    private readonly RequestDelegate _next;

    public MultiSchemeAuthenticationMiddleware(RequestDelegate next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
    }

    public async Task Invoke(HttpContext context, IServiceScopeFactory scopeFactory)
    {
        using var scope = scopeFactory.CreateScope();

        var schemes = scope.ServiceProvider.GetRequiredService<IAuthenticationSchemeProvider>();
        var handlers = scope.ServiceProvider.GetRequiredService<IAuthenticationHandlerProvider>();

        foreach (var scheme in await schemes.GetAllSchemesAsync())
        {
            var handler = await handlers.GetHandlerAsync(context, scheme.Name);
            if (handler is IAuthenticationRequestHandler requestHandler)
            {
                // Actively handle the request for schemes that support it
                if (await requestHandler.HandleRequestAsync())
                {
                    return;
                }
            }
            else if (scheme.Name == JwtBearerDefaults.AuthenticationScheme)
            {
                // Handle JWT bearer authentication explicitly
                var result = await context.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
                if (result?.Principal != null)
                {
                    context.User = result.Principal;
                    break;
                }
            }
        }

        await _next(context);
    }
}


