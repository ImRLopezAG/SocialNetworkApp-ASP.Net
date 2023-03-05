using Microsoft.AspNetCore.Http;
using SocialNet.Core.Application.Core;

namespace SocialNet.Core.Application.ViewModels.SaveVm;

public class SavePostVm : BaseVm {
  public string? Content { get; set; }
  public int UserId { get; set; }
  public string? Image { get; set; }
  // Ignore this property when mapping
  public IFormFile? ImageFile { get; set; }
}
