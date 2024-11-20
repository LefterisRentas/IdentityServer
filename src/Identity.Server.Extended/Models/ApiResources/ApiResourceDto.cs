namespace Identity.Server.Extended.Models.ApiResources;

/// <summary>
/// Represents an API Resource in the Identity Server.
/// </summary>
public class ApiResourceDto
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApiResourceDto"/> class
    /// with default values for collections.
    /// </summary>
    public ApiResourceDto()
    {
        UserClaims = new List<string>();
        Scopes = new List<string>();
        AllowedAccessTokenSigningAlgorithms = new List<string>();
    }

    /// <summary>
    /// Gets or sets the unique identifier for the API resource.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the unique name of the API resource.
    /// This field is required.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the display name for the API resource.
    /// This is typically shown in the UI.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// Gets or sets the description of the API resource.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the API resource is enabled.
    /// Default is <c>true</c>.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the API resource is visible
    /// in the discovery document.
    /// </summary>
    public bool ShowInDiscoveryDocument { get; set; }

    /// <summary>
    /// Gets or sets the list of user claims associated with the API resource.
    /// </summary>
    public List<string> UserClaims { get; set; }

    /// <summary>
    /// Gets or sets the list of allowed access token signing algorithms for this API resource.
    /// </summary>
    public List<string> AllowedAccessTokenSigningAlgorithms { get; set; }

    /// <summary>
    /// Gets or sets the list of scopes associated with the API resource.
    /// </summary>
    public List<string> Scopes { get; set; }
}
