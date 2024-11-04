namespace Identity.Server.Extended.Models.Clients;

public class ClientClaimsDto
{
    public ClientClaimsDto()
    {
        ClientClaims = new List<ClientClaimDto>();
    }

    public List<ClientClaimDto> ClientClaims { get; set; }

    public int TotalCount { get; set; }

    public int PageSize { get; set; }
}