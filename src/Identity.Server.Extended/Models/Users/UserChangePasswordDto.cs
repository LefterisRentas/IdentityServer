using System.ComponentModel.DataAnnotations;

namespace Identity.Server.Extended.Models.Users;

public class UserChangePasswordDto<TKey>
{
    public TKey UserId { get; set; }

    public required string Password { get; set; }

    [Compare(nameof(Password))] public required string ConfirmPassword { get; set; }
}