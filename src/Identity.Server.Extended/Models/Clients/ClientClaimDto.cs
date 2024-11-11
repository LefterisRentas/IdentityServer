using IdentityServer4.EntityFramework.Entities;

namespace Identity.Server.Extended.Models.Clients;

public class ClientClaimDto
{
    public int Id { get; set; }
    
    public required string Type { get; set; }
    
    public required string Value { get; set; }
    
    /// <summary>
    /// Converts the DTO to an entity.
    /// </summary>
    /// <param name="clientId">The client ID. <see cref="Client"/> <see cref="Client.Id"/></param>
    /// <returns></returns>
    public ClientClaim ToEntity(int clientId)
    {
        return new ClientClaim
        {
            Id = Id,
            Type = Type,
            Value = Value,
            ClientId = clientId
        };
    }
}