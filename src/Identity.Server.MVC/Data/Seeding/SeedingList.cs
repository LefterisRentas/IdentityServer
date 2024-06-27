// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using IdentityServer4.Models;

namespace Identity.Server.MVC.Data.Seeding;

public static class SeedingList
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };


    public static IEnumerable<ApiScope> ApiScopes =>
        new[] { Security.ApiScopes.TestApi };

    public static IEnumerable<Client> Clients => Security.Clients.ClientList;
        
    public static IEnumerable<ApiResource> ApiResources => Security.Resources.GetApiResources();
}