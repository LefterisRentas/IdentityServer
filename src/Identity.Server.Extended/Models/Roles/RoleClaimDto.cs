namespace Identity.Server.Extended.Models.Roles;

public class RoleClaimDto<TKey>
{
    public int ClaimId { get; set; }

    public TKey RoleId { get; set; }

    public required string ClaimType { get; set; }


    public required string ClaimValue { get; set; }
}