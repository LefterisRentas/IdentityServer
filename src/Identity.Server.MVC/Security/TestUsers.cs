// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using Identity.Server.MVC.Models;
using IdentityModel;
using Microsoft.AspNetCore.Identity;

namespace Identity.Server.MVC.Security;

public class TestUsers
{
    public static List<ApplicationUser> Users =
    [
        new ApplicationUser
        {
            Id = "818727", UserName = "alice", PasswordHash = "alice",
            Claims = new List<IdentityUserClaim<string>>
            {
                new IdentityUserClaim<string> { ClaimType = JwtClaimTypes.Name, ClaimValue = "Alice Smith", UserId = "818727" },
                new IdentityUserClaim<string> { ClaimType = JwtClaimTypes.GivenName, ClaimValue = "Alice", UserId = "818727" },
                new IdentityUserClaim<string> { ClaimType = JwtClaimTypes.FamilyName, ClaimValue = "Smith", UserId = "818727" },
                new IdentityUserClaim<string> { ClaimType = JwtClaimTypes.Email, ClaimValue = "AliceSmith@email.com", UserId = "818727" },
                new IdentityUserClaim<string> { ClaimType = JwtClaimTypes.EmailVerified, ClaimValue = "true", UserId = "818727" },
                new IdentityUserClaim<string> { ClaimType = JwtClaimTypes.WebSite, ClaimValue = "http://alice.com", UserId = "818727" },
                new IdentityUserClaim<string>
                {
                    ClaimType = JwtClaimTypes.Address, ClaimValue = @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }"
                    , UserId = "818727"},
            },
            TwoFactorEnabled = true
        },

        new ApplicationUser
        {
            Id = "88421113", UserName = "bob", PasswordHash = "bob",
            Claims = new List<IdentityUserClaim<string>>
            {
                new IdentityUserClaim<string> { ClaimType = JwtClaimTypes.Name, ClaimValue = "Bob Smith", UserId = "818727" },
                new IdentityUserClaim<string> { ClaimType = JwtClaimTypes.GivenName, ClaimValue = "Bob", UserId = "818727" },
                new IdentityUserClaim<string> { ClaimType = JwtClaimTypes.FamilyName, ClaimValue = "Smith", UserId = "818727" },
                new IdentityUserClaim<string> { ClaimType = JwtClaimTypes.Email, ClaimValue = "BobSmith@email.com", UserId = "818727" },
                new IdentityUserClaim<string> { ClaimType = JwtClaimTypes.EmailVerified, ClaimValue = "true", UserId = "818727" },
                new IdentityUserClaim<string> { ClaimType = JwtClaimTypes.WebSite, ClaimValue = "http://bob.com", UserId = "818727" },
                new IdentityUserClaim<string>
                {
                    ClaimType = JwtClaimTypes.Address, ClaimValue = @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }"
                    , UserId = "818727"},
                new IdentityUserClaim<string> { ClaimType = "location", ClaimValue = "somewhere", UserId = "818727" },
            }
        }
    ];
}