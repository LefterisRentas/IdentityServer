using System.Security.Claims;
using Identity.Server.Extended.Extensions;
using Identity.Server.Extended.Models.Clients;
using Identity.Server.Extended.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Identity.Server.Extended.Endpoints.Handlers;

internal static class ClientsHandler
{
    internal static async Task<Results<Ok<List<ClientDto>>, NotFound>> GetClients(IClientManager clientManager, string search, int page = 1, int pageSize = 10)
    {
        var result = await clientManager.GetClientsAsync(search, page, pageSize);
        if (result.IsSuccess is false || result.Result is null)
        {
            return  TypedResults.NotFound();
        }

        return TypedResults.Ok(result.Result.Select(ClientDto.FromEntity).ToList());
    }
    
    internal static async Task<Results<Ok<ClientDto>, NotFound>> GetClientById(IClientManager clientManager, string clientId)
    {
        var result = await clientManager.GetClientByIdAsync(clientId);
        if (result.IsSuccess is false)
        {
            return TypedResults.NotFound();
        }
        if (result.Result is null)
        {
            throw new InvalidOperationException("Client not found. But the operation was successful.");
        }
        return TypedResults.Ok(ClientDto.FromEntity(result.Result));
    }
    
    internal static async Task<Results<CreatedAtRoute<ClientDto>, ValidationProblem>> CreateClient(IClientManager clientManager, ClientDto clientDto, ClaimsPrincipal claimsPrincipal)
    {
        var client = ClientEntityExtensions.FromDto(clientDto);
        var result = await clientManager.CreateClientAsync(client, claimsPrincipal);
        if (result.IsSuccess is false && result.ValidationErrors.Count > 0 || result.Result is null)
        {
            return result.ToValidationProblem() ?? throw new InvalidOperationException("Tried to create a client but the operation failed.");
        }
        return TypedResults.CreatedAtRoute(ClientDto.FromEntity(result.Result), nameof(GetClientById), new { clientId = result.Result.ClientId });
    }
    
    internal static async Task<Results<Ok<ClientDto>, ValidationProblem>> UpdateClient(ClientDto clientDto, bool updateClaims, bool updateProperties, IClientManager clientManager, ClaimsPrincipal claimsPrincipal)
    {
        var client = ClientEntityExtensions.FromDto(clientDto);
        var result = await clientManager.UpdateClientAsync(client, claimsPrincipal, updateClaims, updateProperties);
        if (result.IsSuccess is false && result.ValidationErrors.Count > 0 || result.Result is null)
        {
            return result.ToValidationProblem() ?? throw new InvalidOperationException("Tried to update a client but the operation failed.");
        }
        return TypedResults.Ok(ClientDto.FromEntity(result.Result));
    }
    
    internal static async Task<Results<NoContent, ValidationProblem>> DeleteClient(string clientId, IClientManager clientManager, ClaimsPrincipal claimsPrincipal)
    {
        var result = await clientManager.DeleteClientAsync(clientId, claimsPrincipal);
        if (result.IsSuccess is false && result.ValidationErrors.Count > 0)
        {
            return result.ToValidationProblem() ?? throw new InvalidOperationException("Tried to delete a client but the operation failed.");
        }
        return TypedResults.NoContent();
    }
}