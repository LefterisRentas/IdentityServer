namespace Identity.Server.Extended.Models.ApiResources;

public class ApiSecretsDto
{
    public ApiSecretsDto()
    {
        ApiSecrets = new List<ApiSecretDto>();
    }

    public int TotalCount { get; set; }

    public int PageSize { get; set; }

    public List<ApiSecretDto> ApiSecrets { get; set; }
}