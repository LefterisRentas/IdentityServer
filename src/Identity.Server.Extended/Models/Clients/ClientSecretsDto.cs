using IdentityServer4.EntityFramework.Entities;

namespace Identity.Server.Extended.Models.Clients;

public class ClientSecretsDto
{
    public int TotalCount { get; set; }

    public int PageSize { get; set; } = 10;

    public List<ClientSecretDto> ClientSecrets { get; set; } = new List<ClientSecretDto>();
    
    public static ClientSecretsDto FromEntities(List<ClientSecret> clientSecrets, int pageSize = 10, int page = 1)
    {
        var clientSecretsDto = new ClientSecretsDto
        {
            TotalCount = clientSecrets.Count,
            PageSize = pageSize,
            ClientSecrets = clientSecrets.Skip((page - 1) * pageSize).Take(pageSize).Select(ClientSecretDto.FromEntity).ToList()
        };
        return clientSecretsDto;
    }
}