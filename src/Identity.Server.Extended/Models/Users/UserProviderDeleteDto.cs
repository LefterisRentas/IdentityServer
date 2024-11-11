namespace Identity.Server.Extended.Models.Users;

public class UserProviderDeleteDto<TKey>
{
    public TKey UserId { get; set; }

    public string ProviderKey { get; set; }

    public string LoginProvider { get; set; }
}