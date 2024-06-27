﻿using System.Collections.Generic;

namespace Identity.Server.MVC.Quickstart.Account;

public static class ExternalProviderIcons
{
    public static readonly Dictionary<string, string> ProviderIcons = new Dictionary<string, string>
    {
        { "Google", "bi bi-google" },
        { "Facebook", "bi bi-facebook" },
        { "Microsoft", "bi bi-microsoft" },
        // Add more providers and their corresponding icons here
    };
}