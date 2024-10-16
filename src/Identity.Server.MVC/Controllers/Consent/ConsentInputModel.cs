﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;

namespace Identity.Server.MVC.Controllers.Consent;

public class ConsentInputModel
{
    public string Button { get; set; } = default!;
    public IEnumerable<string>? ScopesConsented { get; set; }
    public bool RememberConsent { get; set; }
    public string? ReturnUrl { get; set; }
    public string? Description { get; set; }
}