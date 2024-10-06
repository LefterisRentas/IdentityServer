using System;

namespace Identity.Server.MVC.Models.Account;

public class LockoutViewModel
{
    public DateTimeOffset LockoutEnd { get; set; } // Time when the lockout ends
    public bool IsPermanentLockout { get; set; } // Flag for permanent lockouts (admin action)
}
