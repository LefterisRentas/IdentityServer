namespace Identity.Server.Extended.Models.IdentityResources;

/// <summary>
/// Represents an identity resource in the Identity Server.
/// Identity resources are used to model user-related data (e.g., profile, email, etc.).
/// </summary>
public class IdentityResourceDto
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityResourceDto"/> class.
    /// Sets up default values for user claims.
    /// </summary>
    public IdentityResourceDto()
    {
        UserClaims = new List<string>();
    }

    /// <summary>
    /// Gets or sets the unique identifier for the identity resource.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the identity resource.
    /// This is a required property and serves as a unique identifier for the resource.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the display name for the identity resource.
    /// This is a user-friendly name typically shown in UI.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// Gets or sets the description for the identity resource.
    /// Used to provide additional details or documentation about the resource.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the identity resource is enabled.
    /// Default is true.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the identity resource should appear in the discovery document.
    /// Default is true.
    /// </summary>
    public bool ShowInDiscoveryDocument { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the identity resource is required for the client.
    /// If true, the client must request this resource.
    /// </summary>
    public bool Required { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the identity resource should be emphasized.
    /// Emphasized resources may be visually highlighted in the UI.
    /// </summary>
    public bool Emphasize { get; set; }

    /// <summary>
    /// Gets or sets the list of user claims associated with the identity resource.
    /// These claims define the type of information available in the identity token.
    /// </summary>
    public List<string> UserClaims { get; set; }
}
