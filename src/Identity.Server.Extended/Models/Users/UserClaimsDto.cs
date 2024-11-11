namespace Identity.Server.Extended.Models.Users;

public class UserClaimsDto<TKey>
{
    public UserClaimsDto()
    {
        Claims = new List<UserClaimDto<TKey>>();
    }

    public List<UserClaimDto<TKey>> Claims { get; set; }

    public int TotalCount { get; set; }

    public int PageSize { get; set; }
}