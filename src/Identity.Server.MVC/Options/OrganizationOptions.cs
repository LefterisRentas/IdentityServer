namespace Identity.Server.MVC.Options;

public class OrganizationOptions
{
    public string Name { get; set; } = "Identity Server";
    public string Description { get; set; } = "Another Identity Server";
    public string ContactEmail { get; set; } = "info@example.com";
    public string ContactPhone { get; set; } = "555-555-5555";
    public string ContactAddress { get; set; } = "123 Main St";
    public string ContactCity { get; set; } = "Any town";
    public string ContactState { get; set; } = "NY";
    public string ContactZip { get; set; }  = "12345";
    public string ContactCountry { get; set; } = "US";
    public string LogoUri { get; set; } = "https://example.com/logo.png";
}