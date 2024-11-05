namespace Identity.Server.Extended.Constants;

/// <summary>
/// The extended event ids for identity server operations.
/// </summary>
public static class ExtendedEventIds
{
    /// <summary>
    /// The start of the extended event ids for client operations.
    /// </summary>
    private const int Start = 6000;
    /// <summary>
    /// The creation of a client.
    /// </summary>
    public const int CLIENT_CREATION = Start + 1;
    /// <summary>
    /// The update of a client.
    /// </summary>
    public const int CLIENT_UPDATE = Start + 2;
    /// <summary>
    /// The deletion of a client.
    /// </summary>
    public const int CLIENT_DELETION = Start + 3;
}