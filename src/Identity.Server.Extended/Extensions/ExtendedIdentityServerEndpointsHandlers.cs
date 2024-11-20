using System.Security.Claims;
using Identity.Server.Extended.Endpoints.Handlers;
using Identity.Server.Extended.Models.Clients;
using Identity.Server.Extended.Services.Abstractions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Identity.Server.Extended.Extensions;

public static class ExtendedIdentityServerEndpointsHandlers
{
    public static Task<Results<Ok<ClientsDto>, NotFound>> GetClients(
        IClientManager clientManager, string? search, int page = 1, int pageSize = 10) =>
        ClientsHandler.GetClients(clientManager, search, page, pageSize);

    public static Task<Results<Ok<ClientDto>, NotFound>> GetClientById(
        IClientManager clientManager, string clientId) =>
        ClientsHandler.GetClientById(clientManager, clientId);

    public static Task<Results<CreatedAtRoute<ClientDto>, ValidationProblem>> CreateClient(
        IClientManager clientManager, ClientDto clientDto, ClaimsPrincipal claimsPrincipal) =>
        ClientsHandler.CreateClient(clientManager, clientDto, claimsPrincipal);

    public static Task<Results<Ok<ClientDto>, ValidationProblem>> UpdateClient(
        ClientDto clientDto, bool updateClaims, bool updateProperties, 
        IClientManager clientManager, ClaimsPrincipal claimsPrincipal) =>
        ClientsHandler.UpdateClient(clientDto, updateClaims, updateProperties, clientManager, claimsPrincipal);

    public static Task<Results<NoContent, ValidationProblem>> DeleteClient(
        string clientId, IClientManager clientManager, ClaimsPrincipal claimsPrincipal) =>
        ClientsHandler.DeleteClient(clientId, clientManager, claimsPrincipal);

    public static Task<Results<Ok<ClientSecretsDto>, NotFound>> GetClientSecrets(
        IClientManager clientManager, int clientId, int pageSize = 10, int page = 1) =>
        ClientsHandler.GetClientSecrets(clientManager, clientId, pageSize, page);

    public static Task<Results<Ok<ClientSecretDto>, ValidationProblem>> GetClientSecret(
        IClientManager clientManager, int secretId) =>
        ClientsHandler.GetClientSecret(clientManager, secretId);

    public static Task<Results<CreatedAtRoute<ClientSecretDto>, ValidationProblem>> CreateClientSecret(
        IClientManager clientManager, int clientId, ClientSecretDto clientSecretDto, ClaimsPrincipal claimsPrincipal) =>
        ClientsHandler.CreateClientSecret(clientManager, clientId, clientSecretDto, claimsPrincipal);

    public static Task<Results<NoContent, ValidationProblem>> DeleteClientSecret(
        IClientManager clientManager, int clientId, int secretId, ClaimsPrincipal claimsPrincipal) =>
        ClientsHandler.DeleteClientSecret(clientManager, clientId, secretId, claimsPrincipal);
}
