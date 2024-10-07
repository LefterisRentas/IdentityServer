using IdentityServer4.Events;

namespace Identity.Server.MVC.Events;

public class UserCreationEvent : Event
{
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public string? UserId { get; set; }
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public string? Provider { get; set; }
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public string? ProviderUserId { get; set; }
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public string? SubjectId { get; set; }
    
    // ReSharper disable once UnusedMember.Global
    public UserCreationEvent(string userId, string? provider = null, string? providerUserId = null, string? subjectId = null) : this()
    {
        UserId = userId;
        Provider = provider;
        ProviderUserId = providerUserId;
        SubjectId = subjectId;
    }
    protected UserCreationEvent() : base(EventCategories.Authentication, "User Created", EventTypes.Information, EventIds.UserLoginSuccess)
    {
    }
}