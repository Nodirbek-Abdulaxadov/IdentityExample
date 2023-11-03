using System.ComponentModel.DataAnnotations;

namespace CustomizeIdentity.ViewModels;

public class LoginUserViewModel
{
    [Required, StringLength(100)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required, StringLength(1000)]
    public string Password { get; set; } = string.Empty;
}