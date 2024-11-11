namespace Identity.Server.Extended.Models.Users;

public class UserClaimDto<TKey>
{
    public int ClaimId { get; set; }

    public TKey UserId { get; set; }

    public required string ClaimType { get; set; }

    public required string ClaimValue { get; set; }
}