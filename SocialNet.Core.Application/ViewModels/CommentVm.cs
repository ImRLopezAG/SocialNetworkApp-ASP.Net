using SocialNet.Core.Application.Core;

namespace SocialNet.Core.Application.ViewModels;

public class CommentVm : BaseVm {
  public string? Content { get; set; }
  public int UserId { get; set; }
  public int PostId { get; set; }

  // ignore mapping
  public string UserImg { get; set; } = null!;
  public string UserName { get; set; } = null!;

}