using System.ComponentModel.DataAnnotations;

namespace SocialNet.Core.Application.ViewModels;

public class LoginVm {
  [Required(ErrorMessage = "User name is required")]
  [DataType(DataType.Text)]
  public string UserName { get; set; } = null!;
  [Required(ErrorMessage = "Password is required")]
  [DataType(DataType.Password)]
  public string Password { get; set; } = null!;

}
