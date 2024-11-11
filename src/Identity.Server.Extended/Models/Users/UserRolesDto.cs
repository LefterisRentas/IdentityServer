namespace Identity.Server.Extended.Models.Users;

public class UserRolesDto<TRoleDto>
{
    public UserRolesDto()
    {
        Roles = new List<TRoleDto>();
    }

    public List<TRoleDto> Roles { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }
}