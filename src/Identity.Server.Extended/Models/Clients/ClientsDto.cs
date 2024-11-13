using IdentityServer4.EntityFramework.Entities;

namespace Identity.Server.Extended.Models.Clients;

/// <summary>
/// Pagination for clients
/// </summary>
public class ClientsDto
{

    /// <summary>
    /// Total count of clients
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Page size
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// List of clients
    /// </summary>
    public required List<ClientDto> Clients { get; set; }
    
    public static ClientsDto FromEntities(List<Client> clients, int pageSize = 10, int page = 1)
    {
        var clientSecretsDto = new ClientsDto
        {
            TotalCount = clients.Count,
            PageSize = pageSize,
            Clients = clients.Skip((page - 1) * pageSize).Take(pageSize).Select(ClientDto.FromEntity).ToList()
        };
        return clientSecretsDto;
    }
}