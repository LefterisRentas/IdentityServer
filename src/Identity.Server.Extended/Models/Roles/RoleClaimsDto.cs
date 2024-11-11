namespace Identity.Server.Extended.Models.Roles;

public class RoleClaimsDto<TKey>
{
    public RoleClaimsDto()
    {
        Claims = new List<RoleClaimDto<TKey>>();
    }

    public List<RoleClaimDto<TKey>> Claims { get; set; }

    public int TotalCount { get; set; }

    public int PageSize { get; set; }
}