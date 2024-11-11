namespace Identity.Server.Extended.Models.IdentityResources;

public class IdentityResourcePropertiesDto
{
    public IdentityResourcePropertiesDto()
    {
        IdentityResourceProperties = new List<IdentityResourcePropertyDto>();
    }

    public int TotalCount { get; set; }

    public int PageSize { get; set; }

    public List<IdentityResourcePropertyDto> IdentityResourceProperties { get; set; }
}