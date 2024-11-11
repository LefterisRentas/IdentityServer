using System.ComponentModel.DataAnnotations;

namespace Identity.Server.Extended.Data.Entities;

/// <summary>
/// Defines a claim type that can be used to create claims for users.
/// </summary>
public class ClaimDefinition
{
    /// <summary>
    /// The unique identifier for the claim type.
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// The name of the claim type.
    /// </summary>
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// A short description of the claim type.
    /// </summary>
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether the claim type is editable. Prevents deletion/modification if false
    /// </summary>
    public bool IsEditable { get; set; } = true;
}