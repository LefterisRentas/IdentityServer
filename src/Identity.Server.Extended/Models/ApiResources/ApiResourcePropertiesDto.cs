namespace Identity.Server.Extended.Models.ApiResources;

public class ApiResourcePropertiesDto
{
    public ApiResourcePropertiesDto()
    {
        ApiResourceProperties = new List<ApiResourcePropertyDto>();
    }

    public List<ApiResourcePropertyDto> ApiResourceProperties { get; set; }

    public int TotalCount { get; set; }

    public int PageSize { get; set; }
}