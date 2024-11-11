namespace Identity.Server.Extended.Models.ApiResources;

public class ApiSecretDto
{
    public required string Type { get; set; } = "SharedSecret";

    public int Id { get; set; }

    public string Description { get; set; }

    public required string Value { get; set; }

    public DateTime? Expiration { get; set; }
}