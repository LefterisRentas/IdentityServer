using System.ComponentModel.DataAnnotations;

namespace Identity.Server.Extended.Data.Entities;

/// <summary>
/// The type of grant
/// </summary>
public class GrantType
{
    /// <summary>
    /// The ID of the grant
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// The type of grant
    /// </summary>
    [MaxLength(250)]
    public required string Type { get; set; }
}