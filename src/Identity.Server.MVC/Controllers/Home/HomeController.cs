// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Identity.Server.MVC.Controllers.Home;

[SecurityHeaders]
[AllowAnonymous]
public class HomeController : Controller
{
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger _logger;

    public HomeController(
        IIdentityServerInteractionService interaction,
        IWebHostEnvironment environment,
        ILogger<HomeController> logger)
    {
        _interaction = interaction;
        _environment = environment;
        _logger = logger;
    }

    public Task<IActionResult> Index()
    {

        if (_environment.IsDevelopment())
        {
            // only show in development
            return Task.FromResult<IActionResult>(View());
        }

        _logger.LogInformation("Homepage is disabled in production. Returning 404.");
        return Task.FromResult<IActionResult>(NotFound());
    }

    /// <summary>
    /// Shows the error page
    /// </summary>
    public async Task<IActionResult> Error(string errorId)
    {

        // retrieve error details from Identity.Server.MVC
        var message = await _interaction.GetErrorContextAsync(errorId);
        var vm = new ErrorViewModel(message ?? new ErrorMessage { Error = "An error occurred" });
        if (message != null)
        {
            vm.Error = message;

            if (!_environment.IsDevelopment())
            {
                // only show in development
                message.ErrorDescription = null;
            }
        }

        return View("Error", vm);
    }
    
    public IActionResult PrivacyPolicy()
    {
        return View("PrivacyPolicy");
    }
}