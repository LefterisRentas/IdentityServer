using Identity.Server.Extended.Services.Abstractions;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Identity.Server.Extended.Endpoints.Handlers;

internal static class ClientsHandler
{
    public static async Task<Results<Ok<List<Client>>, NotFound>> GetClients(IClientManager clientManager)
    {
        var result = await clientManager.GetClientsAsync();
        if (result.IsSuccess is false || result.Result is null || result.Result.Count() is 0)
        {
            return  TypedResults.NotFound();
        }

        return TypedResults.Ok(result.Result.ToList());
    }
    
    public static async Task<Results<Ok<Client>, NotFound>> GetClientById(IClientManager clientManager, string clientId)
    {
        var result = await clientManager.GetClientByIdAsync(clientId);
        if (result.IsSuccess is false)
        {
            return TypedResults.NotFound();
        }
        return TypedResults.Ok(result.Result);
    }
}