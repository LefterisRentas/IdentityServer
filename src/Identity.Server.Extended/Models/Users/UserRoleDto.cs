namespace Identity.Server.Extended.Models.Users;

public class UserRoleDto<TKey>
{
    public TKey UserId { get; set; }

    public TKey RoleId { get; set; }
}