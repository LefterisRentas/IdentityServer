using System.Security.Claims;
using Identity.Server.Extended.Constants;
using IdentityServer4.Events;

namespace Identity.Server.Extended.Events;

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
    public ClientCreationEvent(string clientId, string clientName, ClaimsPrincipal creator) : this()
    {
        ClientId = clientId;
        Creator = creator;
        ClientName = clientName;
    }
    
    private ClientCreationEvent() : base(Client, "Client Created", EventTypes.Information, ExtendedEventIds.ClientCreation)
    {
    }
}