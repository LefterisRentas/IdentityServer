using IdentityServer4.EntityFramework.Entities;

namespace Identity.Server.Extended.Models.Clients;

public class ClientSecretsDto
{
    public int TotalCount { get; set; }

    public int PageSize { get; set; } = 10;

    public List<ClientSecretDto> ClientSecrets { get; set; } = new List<ClientSecretDto>();
    
    public static ClientSecretsDto FromEntities(IEnumerable<ClientSecret> clientSecrets, int pageSize = 10, int page = 1)
    {
        var secrets = clientSecrets.ToList();
        var clientSecretsDto = new ClientSecretsDto
        {
            TotalCount = secrets.Count,
            PageSize = pageSize,
            ClientSecrets = secrets.Skip((page - 1) * pageSize).Take(pageSize).Select(ClientSecretDto.FromEntity).ToList()
        };
        return clientSecretsDto;
    }
}