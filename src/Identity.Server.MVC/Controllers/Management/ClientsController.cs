using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Identity.Server.Extended.Constants;
using Identity.Server.Extended.Extensions;
using Identity.Server.Extended.Models.Clients;
using Identity.Server.Extended.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Server.MVC.Controllers.Management;

[Route("management/clients")]
[Authorize(Policy = AuthorizationPolicyConstants.CLIENT_MANAGEMENT_READ)]
public class ClientsController(IClientManager clientManager, IAuthorizationService authorizationService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? search = null)
    {
        // Call the handler to get the clients
        var result = await ExtendedIdentityServerEndpointsHandlers.GetClients(clientManager, search, page, pageSize);

        // Handle the not found case, e.g., show a message or redirect
        if (result is not { Result: Ok<ClientsDto> ok }) return NotFound(); // Or another appropriate action
        // Handle the result based on its type
        // Extract the clients list from the Ok result
        var clients = ok.Value;
        ViewBag.Page = page; // Pass the page number to the view
        ViewBag.MaxPage = GetMaxPage(clients?.TotalCount, pageSize); // Pass the max page number to the view
        await ManagementController.GetManagementNavViewData(authorizationService, User, ViewData); // Get and set the management navigation view data
        return View(clients); // Pass clients to the view

    }
    
    [HttpGet("{clientId}/edit")]
    public async Task<IActionResult> Edit(string clientId)
    {
        // Call the handler to get the client
        var result = await ExtendedIdentityServerEndpointsHandlers.GetClientById(clientManager, clientId);

        // Handle the not found case, e.g., show a message or redirect
        if (result is not { Result: Ok<ClientDto> ok }) return NotFound(); // Or another appropriate action
        // Handle the result based on its type
        // Extract the client from the Ok result
        var client = ok.Value;
        await ManagementController.GetManagementNavViewData(authorizationService, User, ViewData); // Get and set the management navigation view data
        return View(client); // Pass client to the view
    }

    [HttpPost("{clientId}/edit")]
    public async Task<IActionResult> Edit(string clientId, ClientDto client)
    {
        // Call the handler to update the client
        var result = await ExtendedIdentityServerEndpointsHandlers.UpdateClient(client, false, false, clientManager, User);

        // Handle bad request case, e.g., show a message or redirect
        if (result is not { Result: Ok<ClientDto> ok })
        {
            if (result.Result is ValidationProblem errors)
            {
                foreach (var error in errors.ProblemDetails.Errors)
                {
                    ModelState.AddModelError(error.Key, error.Value[0]);
                }

                return View(client);
            }
        }

        // Handle the result based on its type
        // Extract the update client result from the Ok result
        return RedirectToAction(nameof(Index));
    }

    private static int GetMaxPage(int? totalItems, int pageSize)
    {
        if (totalItems == null) return 1;
        return (int) Math.Ceiling((double) totalItems / pageSize);
    }
}