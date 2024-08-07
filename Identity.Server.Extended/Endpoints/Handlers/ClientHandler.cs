using Identity.Server.Extended.Services.Abstractions;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Identity.Server.Extended.Endpoints.Handlers;

internal static class ClientHandler
{
    public static async Task<Results<Ok<List<Client>>, NotFound>> GetClients(IClientManager clientManager)
    {
        var result = await clientManager.GetClientsAsync();
        var clientsList = result.ToList();
        if (clientsList.Count is 0)
        {
            return  TypedResults.NotFound();
        }

        return TypedResults.Ok(clientsList.ToList());
    }
}