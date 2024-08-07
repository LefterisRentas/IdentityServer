using Identity.Server.Extended.Constants;
using Identity.Server.Extended.Endpoints.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Server.Extended.Endpoints;

/// <summary>
/// Clients API endpoints.
/// </summary>
public static class ClientsApi
{
    private static readonly string[] SourceArray = ["clients"];

    /// <summary>
    /// Maps the clients API.
    /// </summary>
    /// <param name="routes"></param>
    /// <returns></returns>
    public static RouteGroupBuilder MapClients(this IEndpointRouteBuilder routes)
    {
        var options = routes.ServiceProvider.GetService<IConfiguration>();
        var group = routes.MapGroup("/api/clients");
        group.WithTags("Clients");
        // Add security requirements, all incoming requests to this API *must*
        // be authenticated with a valid user.
        var allowedScopes = SourceArray.ToArray();
        group.RequireAuthorization(pb => pb.RequireAuthenticatedUser()
            .RequireClaim(ExtendedClaimTypes.Scope, allowedScopes));
        group.WithOpenApi();

        group.MapGet("", ClientHandler.GetClients)
            .WithName(nameof(ClientHandler.GetClients));

        return group;
    }
}