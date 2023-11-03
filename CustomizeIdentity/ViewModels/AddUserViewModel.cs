using System.ComponentModel.DataAnnotations;

namespace CustomizeIdentity.ViewModels;

public class AddUserViewModel
{
    public string FirstName { get; set; } = string.Empty;
    [StringLength(50)]
    public string LastName { get; set; } = string.Empty;
    [Required, StringLength(1000)]
    public string Password { get; set; } = string.Empty;
    [Required, StringLength(100)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}