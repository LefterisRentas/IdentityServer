using System.Security.Claims;
using Identity.Server.Extended.Constants;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Events;

namespace Identity.Server.Extended.Events;

public class ClientUpdateEvent : Event
{
    private const string Client = nameof(Client);
    /// <summary>
    /// The ClaimsPrincipal representing the user.
    /// </summary>
    public ClaimsPrincipal User { get; set; } = null!;

    /// <summary>
    /// The original client.
    /// </summary>
    public Client OriginalClient { get; set; } = null!;

    /// <summary>
    /// The modified client.
    /// </summary>
    public Client ModifiedClient { get; set; } = null!;

    /// <summary>
    /// Constructor for the ClientUpdateEvent.
    /// </summary>
    /// <param name="user">The ClaimsPrincipal representing the user.</param>
    /// <param name="originalClient">The original client.</param>
    /// <param name="modifiedClient">The modified client.</param>
    public ClientUpdateEvent(ClaimsPrincipal user, Client originalClient, Client modifiedClient) : this()
    {
        User = user;
        OriginalClient = originalClient;
        ModifiedClient = modifiedClient;
    }
    
    private ClientUpdateEvent() : base(Client, "Client Updated", EventTypes.Information, ExtendedEventIds.CLIENT_UPDATE)
    {
    }
}