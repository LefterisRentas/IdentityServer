namespace Identity.Server.Extended.Constants;

/// <summary>
/// The two factor providers.
/// </summary>
public enum TwoFactorProviders
{
    /// <summary>
    /// Email provider.
    /// </summary>
    Email,
    /// <summary>
    /// Phone provider.
    /// </summary>
    Phone,
    /// <summary>
    /// None provider.
    /// </summary>
    None = -1
}