namespace Identity.Server.Extended.Models.ApiScopes;

public class ApiScopesDto
{
    public ApiScopesDto()
    {
        Scopes = new List<ApiScopeDto>();
    }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public List<ApiScopeDto> Scopes { get; set; }
}