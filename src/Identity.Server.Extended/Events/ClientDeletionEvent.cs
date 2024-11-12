using System.Security.Claims;
using Identity.Server.Extended.Constants;
using Identity.Server.Extended.Models.Clients;
using IdentityServer4.Events;

namespace Identity.Server.Extended.Events;

public class ClientDeletionEvent : Event
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
    /// The ClaimsPrincipal of the user that deleted the client
    /// </summary>
    public ClaimsPrincipal User { get; set; }
    
    public ClientDto ClientDto { get; set; }
    
    public ClientDeletionEvent(ClientDto clientDto, ClaimsPrincipal user) : this()
    {
        ClientId = clientDto.ClientId;
        ClientName = clientDto.ClientName;
        User = user;
        ClientDto = clientDto;
    }
    
    private ClientDeletionEvent() : base(Client, "Client Deleted", EventTypes.Information, ExtendedEventIds.CLIENT_DELETION)
    {
    }
}