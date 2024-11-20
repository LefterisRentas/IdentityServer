namespace Identity.Server.Extended.Models.ApiScopes;

/// <summary>
/// Represents an API Scope in the Identity Server.
/// </summary>
public class ApiScopeDto
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApiScopeDto"/> class
    /// with default values for collections.
    /// </summary>
    public ApiScopeDto()
    {
        UserClaims = new List<string>();
        ApiScopeProperties = new List<ApiScopePropertyDto>();
    }

    /// <summary>
    /// Gets or sets a value indicating whether the API scope should be shown
    /// in the discovery document. Default is <c>true</c>.
    /// </summary>
    public bool ShowInDiscoveryDocument { get; set; } = true;

    /// <summary>
    /// Gets or sets the unique identifier for the API scope.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the unique name of the API scope.
    /// This field is required.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the display name for the API scope.
    /// This is typically shown in the UI.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// Gets or sets the description of the API scope.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the API scope is required
    /// during the authorization process.
    /// </summary>
    public bool Required { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the API scope should be
    /// emphasized in the UI. Useful for important or sensitive scopes.
    /// </summary>
    public bool Emphasize { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the API scope is enabled.
    /// Default is <c>true</c>.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Gets or sets the list of user claims associated with the API scope.
    /// </summary>
    public List<string> UserClaims { get; set; }

    /// <summary>
    /// Gets or sets the list of properties associated with the API scope.
    /// </summary>
    public List<ApiScopePropertyDto> ApiScopeProperties { get; set; }
}
