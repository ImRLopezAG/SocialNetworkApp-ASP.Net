using Microsoft.AspNetCore.Http;

namespace SocialNet.Core.Application.ViewModels;

public class HomeVm {
  public string? Content { get; set; } = null!;
  public IFormFile? ImageFile { get; set; } = null!;
  public List<PostVm>? Posts { get; set; } = null!;

}
