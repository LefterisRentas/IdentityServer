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
    public required string ClientId;
    
    /// <summary>
    /// The name of the client that was created
    /// </summary>
    public required string ClientName;
    
    /// <summary>
    /// The creator of the client
    /// </summary>
    public ClaimsPrincipal Creator { get; set; }

    /// <summary>
    /// Constructor for <see cref="ClientCreationEvent"/>
    /// </summary>
    /// <param name="clientId">The client id</param>
    /// <param name="clientName"></param>
    /// <param name="creator"></param>
    public ClientCreationEvent(ClaimsPrincipal creator) : this()
    {
        Creator = creator;
    }
    
    private ClientCreationEvent() : base(Client, "Client Created", EventTypes.Information, ExtendedEventIds.CLIENT_CREATION)
    {
    }
}