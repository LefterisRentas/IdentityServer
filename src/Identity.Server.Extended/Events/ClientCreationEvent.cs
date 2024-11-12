using System.Security.Claims;
using Identity.Server.Extended.Constants;
using IdentityServer4.Events;

namespace Identity.Server.Extended.Events;

/// <summary>
/// Event for when a client is created
/// </summary>
public class ClientCreationEvent : Event
{
    private const string Client = nameof(Client);
    
    /// <summary>
    /// The ID of the client that was created
    /// </summary>
    public string ClientId = null!;
    
    /// <summary>
    /// The name of the client that was created
    /// </summary>
    public string ClientName = null!;
    
    /// <summary>
    /// The creator of the client
    /// </summary>
    public ClaimsPrincipal Creator { get; set; } = null!;

    /// <summary>
    /// Constructor for <see cref="ClientCreationEvent"/>
    /// </summary>
    /// <param name="clientId">The client id</param>
    /// <param name="clientName"></param>
    /// <param name="creator"></param>
    public ClientCreationEvent(ClaimsPrincipal creator, string clientId, string clientName) : this()
    {
        Creator = creator;
        ClientId = clientId;
        ClientName = clientName;
    }
    
    private ClientCreationEvent() : base(Client, "Client Created", EventTypes.Information, ExtendedEventIds.CLIENT_CREATION)
    {
    }
}