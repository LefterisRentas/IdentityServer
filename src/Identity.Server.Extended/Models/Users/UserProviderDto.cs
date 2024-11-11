namespace Identity.Server.Extended.Models.Users;

public class UserProviderDto<TKey>
{
    public TKey UserId { get; set; }

    public string UserName { get; set; }

    public string ProviderKey { get; set; }

    public string LoginProvider { get; set; }

    public string ProviderDisplayName { get; set; }
}