namespace Identity.Server.Extended.Constants;

/// <summary>
/// Authorization policy constants.
/// </summary>
public static class AuthorizationPolicyConstants
{
    /// <summary>
    /// Policy for admin users.
    /// </summary>
    public const string IS_ADMIN = nameof(IS_ADMIN);
    /// <summary>
    /// Policy for user management.
    /// </summary>
    public const string USER_MANAGEMENT_READ = nameof(USER_MANAGEMENT_READ);
    /// <summary>
    /// Policy for role management.
    /// </summary>
    public const string ROLE_MANAGEMENT_READ = nameof(ROLE_MANAGEMENT_READ);
    /// <summary>
    /// Policy for scope management.
    /// </summary>
    public const string SCOPE_MANAGEMENT_READ = nameof(SCOPE_MANAGEMENT_READ);
    /// <summary>
    /// Policy for client management.
    /// </summary>
    public const string CLIENT_MANAGEMENT_READ = nameof(CLIENT_MANAGEMENT_READ);
    /// <summary>
    /// Policy for identity resource management.
    /// </summary>
    public const string IDENTITY_RESOURCE_MANAGEMENT_READ = nameof(IDENTITY_RESOURCE_MANAGEMENT_READ);
    /// <summary>
    /// Policy for api resource management.
    /// </summary>
    public const string API_RESOURCE_MANAGEMENT_READ = nameof(API_RESOURCE_MANAGEMENT_READ);
    /// <summary>
    /// Policy for users write.
    /// </summary>
    public const string USER_MANAGEMENT_WRITE = nameof(USER_MANAGEMENT_WRITE);
    /// <summary>
    /// Policy for roles write.
    /// </summary>
    public const string ROLE_MANAGEMENT_WRITE = nameof(ROLE_MANAGEMENT_WRITE);
    /// <summary>
    /// Policy for scopes write.
    /// </summary>
    public const string SCOPE_MANAGEMENT_WRITE = nameof(SCOPE_MANAGEMENT_WRITE);
    /// <summary>
    /// Policy for clients write.
    /// </summary>
    public const string CLIENT_MANAGEMENT_WRITE = nameof(CLIENT_MANAGEMENT_WRITE);
    /// <summary>
    /// Policy for identity resource write.
    /// </summary>
    public const string IDENTITY_RESOURCE_MANAGEMENT_WRITE = nameof(IDENTITY_RESOURCE_MANAGEMENT_WRITE);
    /// <summary>
    /// Policy for api resource write.
    /// </summary>
    public const string API_RESOURCE_MANAGEMENT_WRITE = nameof(API_RESOURCE_MANAGEMENT_WRITE);
}