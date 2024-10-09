using Identity.Server.Extended.Models;
using Identity.Server.Extended.Services.Abstractions;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.Server.Extended.Services;

/// <summary>
/// <inheritdoc cref="IClientManager"/>
/// </summary>
public class ClientManager : IClientManager
{
    private readonly ConfigurationDbContext _context;
    
    /// <summary>
    /// <inheritdoc cref="IClientManager"/>
    /// </summary>
    public ClientManager(ConfigurationDbContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// <inheritdoc cref="IClientManager.GetClientsAsync"/>
    /// </summary>
    /// <returns></returns>
    public Task<OperationResult<IEnumerable<Client>?>> GetClientsAsync()
    {
        var clients = _context.Clients.AsNoTracking().AsEnumerable();
        return Task.FromResult(OperationResult.Success(clients));
    }

    /// <summary>
    /// <inheritdoc cref="IClientManager.GetClientByIdAsync"/>
    /// </summary>
    public Task<OperationResult<Client?>> GetClientByIdAsync(string clientId)
    {
        var client = _context.Clients.AsNoTracking().FirstOrDefault(c => c.ClientId == clientId);
        return Task.FromResult(OperationResult.Success(client));
    }

    /// <summary>
    /// <inheritdoc cref="IClientManager.CreateClientAsync"/>
    /// </summary>
    /// <param name="client"></param>
    /// <returns></returns>
    public async Task<OperationResult<Client?>> CreateClientAsync(Client client)
    {
        var validationResult = ValidateClient(client, ClientAction.Create);
        if (!validationResult.IsValid)
        {
            return OperationResult.Failure<Client?>(validationResult.ValidationErrors);
        }
        _context.Clients.Add(client);
        await _context.SaveChangesAsync();
        return OperationResult.Success(client);
    }
    /// <summary>
    /// <inheritdoc cref="IClientManager.UpdateClientAsync"/>
    /// </summary>
    /// <param name="client"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<OperationResult<Client>> UpdateClientAsync(Client client)
    {
        var validationResult = ValidateClient(client, ClientAction.Update);
        if (!validationResult.IsValid)
        {
            return Task.FromResult(OperationResult.Failure<Client>(validationResult.ValidationErrors));
        }
        throw new NotImplementedException();
    }
    /// <summary>
    /// <inheritdoc cref="IClientManager.DeleteClientAsync"/>
    /// </summary>
    /// <param name="clientId"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<OperationResult> DeleteClientAsync(string clientId)
    {
        var validationResult = ValidateClient(new Client {ClientId = clientId}, ClientAction.Delete);
        if (!validationResult.IsValid)
        {
            return OperationResult.Failure(validationResult.ValidationErrors);
        }
        var client = _context.Clients.FirstOrDefault(c => c.ClientId == clientId);
        if (client is null)
        {
            return OperationResult.Failure("Client not found.");
        }
        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
        return OperationResult.Success();
    }
    
    private ValidationResult ValidateClient(Client client, ClientAction action)
    {
        var errors = new List<string>();
        switch (action)
        {
            case ClientAction.Create:
            {
                if (string.IsNullOrWhiteSpace(client.ClientId))
                {
                    errors.Add("Client Id is required.");
                }
                if (string.IsNullOrWhiteSpace(client.ClientName))
                {
                    errors.Add("Client Name is required.");
                }

                break;
            }
            case ClientAction.Update:
            {
                if (string.IsNullOrWhiteSpace(client.ClientId))
                {
                    errors.Add("Client Id is required.");
                }
                if (string.IsNullOrWhiteSpace(client.ClientName))
                {
                    errors.Add("Client Name is required.");
                }

                break;
            }
            case ClientAction.Delete:
            {
                if (string.IsNullOrWhiteSpace(client.ClientId))
                {
                    errors.Add("Client Id is required.");
                }

                break;
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(action), action, null);
        }
        return new ValidationResult(errors.Count == 0, errors.ToArray());
    }
    private enum ClientAction
    {
        Create,
        Update,
        Delete
    
    }
}