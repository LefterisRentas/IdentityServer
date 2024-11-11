using IdentityServer4.EntityFramework.Entities;

namespace Identity.Server.Extended.Models.Clients;

public class ClientPropertyDto
{
    public int Id { get; set; }
    public required string Key { get; set; }
    public required string Value { get; set; }
    
    /// <summary>
    /// Converts the DTO to an entity.
    /// </summary>
    /// <param name="clientId">The client ID. <see cref="Client"/> <see cref="Client.Id"/></param>
    /// <returns></returns>
    public ClientProperty ToEntity(int clientId)
    {
        return new ClientProperty
        {
            Id = Id,
            Key = Key,
            Value = Value,
            ClientId = clientId
        };
    }
}