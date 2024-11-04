namespace Identity.Server.Extended.Models.Clients;

public class ClientClaimDto
{
    public int Id { get; set; }
    
    public required string Type { get; set; }
    
    public required string Value { get; set; }
}