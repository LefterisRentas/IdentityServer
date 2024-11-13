using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Identity.Server.Extended.Constants;
using Identity.Server.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Serilog.Events;
using Serilog.Parsing;

namespace Identity.Server.MVC.Controllers.Management;

[Authorize(Policy = AuthorizationPolicyConstants.CAN_VIEW_MANAGEMENT_PAGE)]
[Route("management")]
public class ManagementController(IAuthorizationService authorizationService, UserManager<ApplicationUser> userManager) : Controller
{
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        await GetManagementNavViewData(authorizationService, User, ViewData);

        return View();
    }

    public static async Task GetManagementNavViewData(IAuthorizationService authorizationService, ClaimsPrincipal user, ViewDataDictionary viewData)
    {
        viewData["CanManageClients"] = await CanManageClients(user, authorizationService);
        viewData["CanManageUsers"] = await CanManageApiResources(user, authorizationService);
        viewData["CanManageRoles"] = await CanManageRoles(user, authorizationService);
        viewData["CanManageScopes"] = await CanManageScopes(user, authorizationService);
        viewData["CanManageIdentityResources"] = await CanManageIdentityResources(user, authorizationService);
        viewData["CanManageApiResources"] = await CanManageApiResources(user, authorizationService);
    }

    private static async Task<bool> CanManageClients(ClaimsPrincipal user, IAuthorizationService authorizationService)
    {
        return (await authorizationService.AuthorizeAsync(user, AuthorizationPolicyConstants.CLIENT_MANAGEMENT_READ)).Succeeded;
    }
    
    private static async Task<bool> CanManageUsers(ClaimsPrincipal user, IAuthorizationService authorizationService)
    {
        return (await authorizationService.AuthorizeAsync(user, AuthorizationPolicyConstants.USER_MANAGEMENT_READ)).Succeeded;
    }
    
    private static async Task<bool> CanManageRoles(ClaimsPrincipal user, IAuthorizationService authorizationService)
    {
        return (await authorizationService.AuthorizeAsync(user, AuthorizationPolicyConstants.ROLE_MANAGEMENT_READ)).Succeeded;
    }
    
    private static async Task<bool> CanManageScopes(ClaimsPrincipal user, IAuthorizationService authorizationService)
    {
        return (await authorizationService.AuthorizeAsync(user, AuthorizationPolicyConstants.SCOPE_MANAGEMENT_READ)).Succeeded;
    }
    
    private static async Task<bool> CanManageIdentityResources(ClaimsPrincipal user, IAuthorizationService authorizationService)
    {
        return (await authorizationService.AuthorizeAsync(user, AuthorizationPolicyConstants.IDENTITY_RESOURCE_MANAGEMENT_READ)).Succeeded;
    }
    
    private static async Task<bool> CanManageApiResources(ClaimsPrincipal user, IAuthorizationService authorizationService)
    {
        return (await authorizationService.AuthorizeAsync(user, AuthorizationPolicyConstants.API_RESOURCE_MANAGEMENT_READ)).Succeeded;
    }
}