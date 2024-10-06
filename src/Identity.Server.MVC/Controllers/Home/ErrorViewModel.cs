// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;

namespace Identity.Server.MVC.Controllers.Home;

public class ErrorViewModel(ErrorMessage error)
{
    public ErrorViewModel(string error) : this(new ErrorMessage { Error = error })
    {
    }

    public ErrorMessage Error { get; set; } = error;
}