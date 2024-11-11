namespace Identity.Server.Extended.Models.Users;

public class UserProvidersDto<TKey>
{
    public UserProvidersDto()
    {
        Providers = new List<UserProviderDto<TKey>>();
    }

    public List<UserProviderDto<TKey>> Providers { get; set; }
}