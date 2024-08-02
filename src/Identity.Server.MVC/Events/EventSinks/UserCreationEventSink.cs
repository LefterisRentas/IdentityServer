using System.Threading.Tasks;
using Identity.Server.MVC.Controllers.Account;
using Identity.Server.MVC.Models;
using Identity.Server.MVC.Services.Abstractions;
using IdentityServer4.Events;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;

namespace Identity.Server.MVC.Events.EventSinks;

public class UserCreationEventSink : IEventSink
{
    private readonly IEmailService _emailService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUrlHelperFactory _urlHelperFactory;
    
    public UserCreationEventSink(IEmailService emailService, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IUrlHelperFactory urlHelperFactory)
    {
        _emailService = emailService;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _urlHelperFactory = urlHelperFactory;
    }
    
    public async Task PersistAsync(Event evt)
    {
        if (evt is UserCreationEvent userCreationEvent)
        {
            var user = _userManager.FindByIdAsync(userCreationEvent.UserId).Result;
            if (user != null)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var httpContext = _httpContextAccessor.HttpContext;
                var urlHelper = _urlHelperFactory.GetUrlHelper(new ActionContext(httpContext, httpContext.GetRouteData(), new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()));
                var confirmationLink = urlHelper.Action(nameof(AccountController.ConfirmEmail), "Account", new { token, email = user.Email }, httpContext.Request.Scheme);


                await _emailService.SendEmailAsync([user.Email], 
                    null, 
                    null, 
                    "Confirm your email", 
                    $"Please confirm your account by clicking this <a href='{confirmationLink}'>link</a>");
            }
        }
    }
}