using IdentityServer4;

namespace Identity.Server.Extended.Models.ApiResources;

/// <summary>
/// Represents a secret used to secure API resources in the Identity Server.
/// API secrets are used for authentication between clients and APIs.
/// </summary>
public class ApiSecretDto
{
    /// <summary>
    /// Gets or sets the type of the secret.
    /// The default value is "SharedSecret".
    /// Common types include "SharedSecret" or "X509CertificateThumbprint".
    /// </summary>
    public required string Type { get; set; } = IdentityServerConstants.SecretTypes.SharedSecret;

    /// <summary>
    /// Gets or sets the unique identifier for the API secret.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets a description of the secret.
    /// This can be used to provide additional information or documentation about the secret.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the value of the secret.
    /// The value is required and typically hashed for security purposes.
    /// </summary>
    public required string Value { get; set; }

    /// <summary>
    /// Gets or sets the expiration date of the secret.
    /// If set, the secret becomes invalid after this date.
    /// </summary>
    public DateTime? Expiration { get; set; }
}