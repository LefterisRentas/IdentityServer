using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Identity.Server.Extended.Constants;
using Identity.Server.Extended.Extensions;
using Identity.Server.Extended.Models.Clients;
using Identity.Server.Extended.Services.Abstractions;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;

namespace Identity.Server.MVC.Controllers.Management;

[Route("management/clients")]
[Authorize(Policy = AuthorizationPolicyConstants.CLIENT_MANAGEMENT_READ)]
public class ClientsController(IClientManager clientManager, IAuthorizationService authorizationService, ILogger<ClientsController> logger) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? search = null) {
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

    [HttpGet("create")]
    [Authorize(Policy = AuthorizationPolicyConstants.CLIENT_MANAGEMENT_WRITE)]
    public async Task<IActionResult> Create() {
        await ManagementController.GetManagementNavViewData(authorizationService, User, ViewData); // Get and set the management navigation view data
        return View(new ClientDto { ClientId = string.Empty, ClientName = string.Empty }); // Pass a new client to the view
    }

    [HttpPost("create")]
    [Authorize(Policy = AuthorizationPolicyConstants.CLIENT_MANAGEMENT_WRITE)]
    public async Task<IActionResult> Create(ClientDto client) {
        // Call the handler to create the client
        var result = await ExtendedIdentityServerEndpointsHandlers.CreateClient(clientManager, client, User);

        // Handle bad request case, e.g., show a message or redirect
        if (result is not { Result: CreatedAtRoute<ClientDto> }) return BadRequest(); // Or another appropriate action
        // Handle the result based on its type
        // Extract the created client from the Created result
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("{clientId}/edit")]
    public async Task<IActionResult> Edit(string clientId) {
        // Call the handler to get the client
        var result = await ExtendedIdentityServerEndpointsHandlers.GetClientById(clientManager, clientId);

        // Handle the not found case, e.g., show a message or redirect
        if (result is not { Result: Ok<ClientDto> ok }) return NotFound(); // Or another appropriate action
        // Handle the result based on its type
        // Extract the client from the Ok result
        var client = ok.Value;
        await ManagementController.GetManagementNavViewData(authorizationService, User, ViewData); // Get and set the management navigation view data
        client!.LastAccessed = DateTime.UtcNow;
        return View(client); // Pass client to the view
    }

    [HttpPost("{clientId}/edit")]
    [Authorize(Policy = AuthorizationPolicyConstants.CLIENT_MANAGEMENT_WRITE)]
    public async Task<IActionResult> Edit(string clientId, ClientDto client) {
        // Call the handler to update the client
        var result = await ExtendedIdentityServerEndpointsHandlers.UpdateClient(client, true, true, clientManager, User);

        // Handle bad request case, e.g., show a message or redirect
        if (result is not { Result: Ok<ClientDto> }) {
            if (result.Result is ValidationProblem errors) {
                foreach (var error in errors.ProblemDetails.Errors) {
                    ModelState.AddModelError(error.Key, error.Value[0]);
                }

                return View(client);
            }
        }

        // Handle the result based on its type
        // Extract the update client result from the Ok result
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("{clientId}/delete")]
    [Authorize(Policy = AuthorizationPolicyConstants.CLIENT_MANAGEMENT_WRITE)]
    public async Task<IActionResult> Delete(string clientId) {
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

    [HttpPost("{clientId}/delete")]
    [Authorize(Policy = AuthorizationPolicyConstants.CLIENT_MANAGEMENT_WRITE)]
    public async Task<IActionResult> Delete(string clientId, ClientDto client) {
        // Call the handler to delete the client
        var result = await ExtendedIdentityServerEndpointsHandlers.DeleteClient(clientId, clientManager, User);

        // Handle bad request case, e.g., show a message or redirect
        if (result is not { Result: NoContent _ }) return BadRequest(); // Or another appropriate action
        // Handle the result based on its type
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("{clientId}/secrets")]
    public async Task<IActionResult> Secrets(int clientId, int page = 1, int pageSize = 10, string clientName = "") {
        // Call the handler to get the client secrets
        var result = await ExtendedIdentityServerEndpointsHandlers.GetClientSecrets(clientManager, clientId, pageSize, page);

        // Handle the not found case, e.g., show a message or redirect
        if (result is not { Result: Ok<ClientSecretsDto> ok }) return NotFound(); // Or another appropriate action
        // Handle the result based on its type
        // Extract the client secrets from the Ok result
        var secrets = ok.Value;
        ViewBag.Page = page; // Pass the page number to the view
        ViewBag.MaxPage = GetMaxPage(secrets?.TotalCount, pageSize); // Pass the max page number to the view
        ViewBag.Id = clientId; // Pass the client ID to the view. The id is the actual database ID of the client
        ViewBag.ClientId = clientName; // Pass the client name to the view for display
        await ManagementController.GetManagementNavViewData(authorizationService, User, ViewData); // Get and set the management navigation view data
        return View(secrets); // Pass secrets to the view
    }

    [HttpGet("{clientId}/secrets/create")]
    [Authorize(Policy = AuthorizationPolicyConstants.CLIENT_MANAGEMENT_WRITE)]
    public async Task<IActionResult> CreateSecret(int clientId) {
        await ManagementController.GetManagementNavViewData(authorizationService, User, ViewData); // Get and set the management navigation view data
        ViewBag.ClientId = clientId; // Pass the client ID to the view
        return View(new ClientSecretDto { Type = IdentityServerConstants.SecretTypes.SharedSecret, HashType = ClientSecretHashType.Sha256, Value = "" }); // Pass a new client secret to the view
    }

    [HttpPost("{clientId}/secrets/create")]
    [Authorize(Policy = AuthorizationPolicyConstants.CLIENT_MANAGEMENT_WRITE)]
    public async Task<IActionResult> CreateSecret(int clientId, ClientSecretDto secret) {
        // Call the handler to create the client secret
        var result = await ExtendedIdentityServerEndpointsHandlers.CreateClientSecret(clientManager, clientId, secret, User);

        // Handle bad request case, e.g., show a message or redirect
        if (result is not { Result: CreatedAtRoute<ClientSecretDto> }) return BadRequest(); // Or another appropriate action
        // Handle the result based on its type
        // Extract the created client secret from the Created result
        return RedirectToAction(nameof(Secrets), new { clientId });
    }

    [HttpGet("{clientId}/secrets/{secretId}/delete")]
    [Authorize(Policy = AuthorizationPolicyConstants.CLIENT_MANAGEMENT_WRITE)]
    public async Task<IActionResult> DeleteSecret(int clientId, int secretId) {
        // Call the handler to get the client secret
        var result = await ExtendedIdentityServerEndpointsHandlers.GetClientSecret(clientManager, secretId);

        // Handle the not found case, e.g., show a message or redirect
        if (result is not { Result: Ok<ClientSecretDto> ok }) return NotFound(); // Or another appropriate action
        // Handle the result based on its type
        // Extract the client secret from the Ok result
        var secret = ok.Value;
        ViewBag.ClientId = clientId; // Pass the client ID to the view
        await ManagementController.GetManagementNavViewData(authorizationService, User, ViewData); // Get and set the management navigation view data
        return View(secret); // Pass secret to the view
    }

    [HttpPost("{clientId}/secrets/{secretId}/delete")]
    [Authorize(Policy = AuthorizationPolicyConstants.CLIENT_MANAGEMENT_WRITE)]
    public async Task<IActionResult> DeleteSecret(int clientId, int secretId, ClientSecretDto secret) {
        // Call the handler to delete the client secret
        var result = await ExtendedIdentityServerEndpointsHandlers.DeleteClientSecret(clientManager, clientId, secretId, User);

        // Handle validation problems
        if (result is { Result: ValidationProblem validationProblem }) {
            var validationErrors = validationProblem.ProblemDetails.Extensions;

            // Create a new ModelStateDictionary
            var modelState = new ModelStateDictionary();

            // Safely iterate through the extensions
            foreach (var (key, value) in validationErrors) {
                if (value is IEnumerable<string> errors) // Ensure value is a collection of strings
                {
                    foreach (var error in errors) {
                        if (!string.IsNullOrWhiteSpace(error)) // Ensure the error is non-empty
                        {
                            modelState.AddModelError(key, error);
                        }
                    }
                } else {
                    // Log or handle unexpected value types
                    logger.LogError("Unexpected value type for key: {key}. Value: {value}", key, value);
                }
            }

            return ValidationProblem(
                detail: validationProblem.ProblemDetails.Detail,
                instance: validationProblem.ProblemDetails.Instance,
                statusCode: validationProblem.ProblemDetails.Status,
                title: validationProblem.ProblemDetails.Title,
                type: validationProblem.ProblemDetails.Type,
                modelStateDictionary: modelState
            );
        }

        // Handle unexpected results
        if (result is not { Result: NoContent _ }) {
            return BadRequest("An unexpected error occurred while deleting the client secret.");
        }

        // Redirect to the secrets page if successful
        return RedirectToAction(nameof(Secrets), new { clientId });
    }


    private static int GetMaxPage(int? totalItems, int pageSize) {
        if (totalItems == null) return 1;
        return (int)Math.Ceiling((double)totalItems / pageSize);
    }
}