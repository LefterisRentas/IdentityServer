using IdentityServer4.Events;

namespace Identity.Server.MVC.Events;

public class UserCreationEvent : Event
{
    public string? UserId { get; set; }
    public string? Provider { get; set; }
    public string? ProviderUserId { get; set; }
    public string? SubjectId { get; set; }
    
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