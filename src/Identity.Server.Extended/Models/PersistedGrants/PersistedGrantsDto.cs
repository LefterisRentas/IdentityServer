namespace Identity.Server.Extended.Models.PersistedGrants;

public class PersistedGrantsDto
{
    public PersistedGrantsDto()
    {
        PersistedGrants = new List<PersistedGrantDto>();
    }

    public int TotalCount { get; set; }

    public int PageSize { get; set; }

    public List<PersistedGrantDto> PersistedGrants { get; set; }
}