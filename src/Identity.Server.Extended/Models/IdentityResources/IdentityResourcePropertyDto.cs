namespace Identity.Server.Extended.Models.IdentityResources;

/// <summary>
/// Represents a property associated with an identity resource in the Identity Server.
/// </summary>
public class IdentityResourcePropertyDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the identity resource property.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the key for the property.
    /// This typically represents the name or identifier of the property.
    /// </summary>
    public required string Key { get; set; }

    /// <summary>
    /// Gets or sets the value associated with the property key.
    /// </summary>
    public required string Value { get; set; }
}