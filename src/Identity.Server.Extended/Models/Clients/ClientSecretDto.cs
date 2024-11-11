using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Models;

namespace Identity.Server.Extended.Models.Clients;

public class ClientSecretDto
{
    public required string Type { get; set; } = "SharedSecret";

    public int Id { get; set; }

    public string Description { get; set; }
    
    public required string Value { get; set; }

    public ClientSecretHashType HashType { get; set; }

    public DateTime? Expiration { get; set; }
    
    public ClientSecret ToEntity(int clientId)
    {
        return new ClientSecret
        {
            Id = Id,
            Description = Description,
            Value = HashClientSecret(),
            Expiration = Expiration,
            ClientId = clientId,
            Type = Type,
            Created = DateTime.UtcNow,
        };
    }
    
    private string HashClientSecret()
    {
        if(Type is not "SharedSecret")
        {
            return Value;
        }

        return HashType switch
        {
            ClientSecretHashType.Sha256 => Value.Sha256(),
            ClientSecretHashType.Sha512 => Value.Sha512(),
            _ => Value
        };
    }
    
    public static ClientSecretDto FromEntity(ClientSecret entity)
    {
        return new ClientSecretDto
        {
            Id = entity.Id,
            Description = entity.Description,
            Value = entity.Value,
            Expiration = entity.Expiration,
            Type = entity.Type,
            HashType = ClientSecretHashType.Unavailable
        };
    }
}
