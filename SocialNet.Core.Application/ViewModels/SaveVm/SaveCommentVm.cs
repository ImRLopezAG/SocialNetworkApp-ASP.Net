using SocialNet.Core.Application.Core;

namespace SocialNet.Core.Application.ViewModels.SaveVm;

public class SaveCommentVm : BaseVm {
  public string Content { get; set; } = null!;
  public int PostId { get; set; }
  public int UserId { get; set; }
}
