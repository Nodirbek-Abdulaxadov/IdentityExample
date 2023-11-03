using System.ComponentModel.DataAnnotations;

namespace CustomizeIdentity.Models;

public class User
{
    [Key, Required]
    public Guid Id { get; set; }
    [Required, StringLength(50)]
    public string FirstName { get; set; } = string.Empty;
    [StringLength(50)]
    public string LastName { get; set; } = string.Empty;
    [Required, StringLength(50)]  
    public string UserName { get; set; } = string.Empty;
    [Required, StringLength(1000)]
    public string PasswordHash { get; set; } = string.Empty;
    [Required, StringLength(100)]
    public string Email { get; set; } = string.Empty;
}