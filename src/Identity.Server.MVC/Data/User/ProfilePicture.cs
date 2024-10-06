using System.ComponentModel.DataAnnotations;

namespace Identity.Server.MVC.Data.User;

public class ProfilePicture
{
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(255)]
    public string FileName { get; set; } = string.Empty;
    [Required]
    [StringLength(100)]
    public string ContentType { get; set; } = string.Empty;
    [Required]
    public byte[] Data { get; set; } = new byte[0];
}