using Microsoft.AspNetCore.Routing;

namespace Identity.Server.Extended.Endpoints;

/// <summary>
/// Identity Resource Management API endpoints.
/// </summary>
public static class IdentityResourcesApi
{
    /// <summary>
    /// Maps the identity resource management API.
    /// </summary>
    /// <param name="routes"></param>
    /// <returns></returns>
    public static void MapIdentityResourcesManagement(this IEndpointRouteBuilder routes)
    {
        // //var options = routes.ServiceProvider.GetService<IConfiguration>();
        // var readClientsGroup = routes.MapGroup(API_PREFIXES.CLIENTS_API_PREFIX);
        // readClientsGroup.WithTags("Clients");
        // // Add security requirements, all incoming requests to this API *must*
        // // be authenticated with a valid user.
        // //TODO: Extract this authorization policy to a shared location.
        // readClientsGroup.RequireAuthorization(AuthorizationPolicyConstants.CLIENT_MANAGEMENT);
        // readClientsGroup.WithOpenApi();
        //
        // readClientsGroup.MapGet("", ClientHandler.GetClients)
        //     .WithName(nameof(ClientHandler.GetClients));
        //
        // readClientsGroup.MapGet("/{clientId:minlength(1)}", ClientHandler.GetClientById)
        //     .WithName(nameof(ClientHandler.GetClientById));
    }
}