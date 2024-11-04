namespace Identity.Server.Extended.Models.Clients;

public class ClientPropertyDto
{
    public int Id { get; set; }
    public required string Key { get; set; }
    public required string Value { get; set; }
}