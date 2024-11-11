namespace Identity.Server.Extended.Models.ApiScopes;

public class ApiScopePropertiesDto
{
    public ApiScopePropertiesDto()
    {
        ApiScopeProperties = new List<ApiScopePropertyDto>();
    }

    public List<ApiScopePropertyDto> ApiScopeProperties { get; set; } = new();

    public int TotalCount { get; set; }

    public int PageSize { get; set; }
}