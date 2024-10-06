using System;
using System.Threading.Tasks;
using Identity.Server.MVC.Controllers.Account;
using Identity.Server.MVC.Models;
using Identity.Server.MVC.Services.Abstractions;
using IdentityServer4.Events;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace Identity.Server.MVC.Events.EventSinks;

public class UserCreationEventSink : IEventSink
{
    private readonly IEmailService _emailService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUrlHelperFactory _urlHelperFactory;
    private readonly ILogger<UserCreationEventSink> _logger;
    
    public UserCreationEventSink(IEmailService emailService, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IUrlHelperFactory urlHelperFactory, ILogger<UserCreationEventSink> logger)
    {
        _emailService = emailService;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _urlHelperFactory = urlHelperFactory;
        _logger = logger;
    }
    
    public async Task PersistAsync(Event evt)
    {
        try
        {
            if (evt is UserCreationEvent userCreationEvent)
            {
                var user = await _userManager.FindByIdAsync(userCreationEvent.UserId ?? throw new ArgumentNullException(nameof(userCreationEvent.UserId)));
                if (user != null)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var httpContext = _httpContextAccessor.HttpContext;
                    var urlHelper = _urlHelperFactory.GetUrlHelper(new ActionContext(httpContext ?? throw new ArgumentNullException(nameof(httpContext)), httpContext.GetRouteData(), new ActionDescriptor()));
                    var confirmationLink = urlHelper.Action(nameof(AccountController.ConfirmEmail), "Account", new { token, email = user.Email }, httpContext.Request.Scheme);


                    await _emailService.SendEmailAsync([user.Email ?? throw new ArgumentNullException(nameof(user.Email))],
                        null,
                        null,
                        "Confirm your email",
                        $"Please confirm your account by clicking this <a href='{confirmationLink}'>link</a>");
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while sending email confirmation email");
            // Log the error and rethrow to return a 500 error to the client
            throw;
        }
    }
}