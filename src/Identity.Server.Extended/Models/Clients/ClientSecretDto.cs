using IdentityServer4;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Models;
using static IdentityServer4.IdentityServerConstants.SecretTypes;

namespace Identity.Server.Extended.Models.Clients;

/// <summary>
/// Data Transfer Object (DTO) representing a client secret. 
/// This DTO is used for transferring client secret data between layers and includes logic for 
/// converting to and from the underlying entity representation.
/// </summary>
public class ClientSecretDto
{
    /// <summary>
    /// The type of the client secret, typically "SharedSecret".
    /// </summary>
    public required string Type { get; set; } = SharedSecret;

    /// <summary>
    /// The unique identifier of the client secret.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// An optional description for the client secret.
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// The actual value of the secret. This may be hashed depending on the hash type.
    /// </summary>
    public required string Value { get; set; }

    /// <summary>
    /// Specifies the hashing algorithm used for the client secret, such as SHA-256 or SHA-512.
    /// </summary>
    public ClientSecretHashType HashType { get; set; }

    /// <summary>
    /// The optional expiration date for the client secret.
    /// </summary>
    public DateTime? Expiration { get; set; }
    
    /// <summary>
    /// Converts the current DTO instance into a <see cref="ClientSecret"/> entity for persistence.
    /// </summary>
    /// <param name="clientId">The ID of the client associated with this secret.</param>
    /// <returns>A <see cref="ClientSecret"/> entity populated with the data from this DTO.</returns>
    public ClientSecret ToEntity(int clientId)
    {
        return new ClientSecret
        {
            Id = Id,
            Description = Description,
            Value = HashClientSecret(), // Hash the secret if required
            Expiration = Expiration?.ToUniversalTime(),
            ClientId = clientId,
            Type = Type,
            Created = DateTime.UtcNow, // Use the current time as the creation timestamp
        };
    }
    
    /// <summary>
    /// Hashes the client secret value based on the specified <see cref="HashType"/>. 
    /// If the secret type is not "SharedSecret", the value is returned as-is.
    /// </summary>
    /// <returns>The hashed client secret value, or the original value if hashing is not applicable.</returns>
    private string HashClientSecret()
    {
        if (Type is not SharedSecret)
        {
            return Value; // Return as-is for non-shared secrets
        }

        return HashType switch
        {
            ClientSecretHashType.Sha256 => Value.Sha256(), // Hash using SHA-256
            ClientSecretHashType.Sha512 => Value.Sha512(), // Hash using SHA-512
            _ => Value // Return the original value if no valid hash type is specified
        };
    }
    
    /// <summary>
    /// Converts a <see cref="ClientSecret"/> entity into a <see cref="ClientSecretDto"/> instance.
    /// </summary>
    /// <param name="entity">The <see cref="ClientSecret"/> entity to convert.</param>
    /// <returns>A <see cref="ClientSecretDto"/> representing the entity data.</returns>
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
