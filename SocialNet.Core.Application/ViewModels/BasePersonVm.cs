using SocialNet.Core.Application.Core;

namespace SocialNet.Core.Application.ViewModels;

public class BasePersonVm : BaseVm {
  public string Name { get; set; } = null!;
  public string LastName { get; set; } = null!;
  public string Email { get; set; } = null!;
  public string Address { get; set; } = null!;
}
