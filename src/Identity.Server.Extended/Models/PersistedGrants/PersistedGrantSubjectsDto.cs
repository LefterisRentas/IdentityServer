namespace Identity.Server.Extended.Models.PersistedGrants;

public class PersistedGrantSubjectsDto
{
    public PersistedGrantSubjectsDto()
    {
        PersistedGrants = new List<PersistedGrantSubjectDto>();
    }

    public int TotalCount { get; set; }

    public int PageSize { get; set; }

    public List<PersistedGrantSubjectDto> PersistedGrants { get; set; }
}