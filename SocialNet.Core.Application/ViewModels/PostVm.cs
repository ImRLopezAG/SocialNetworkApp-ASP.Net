using SocialNet.Core.Application.Core;
using System.ComponentModel.DataAnnotations;

namespace SocialNet.Core.Application.ViewModels;

public class PostVm : BaseVm {
  public string? Content { get; set; }
  public string? Image { get; set; }
  public int UserId { get; set; }

  // ignore mapping
  [Required(ErrorMessage = "Comment cannot be empty")]
  public string Comment { get; set; } = null!;
  public string UserImg { get; set; } = null!;
  public string UserName { get; set; } = null!;
  public ICollection<CommentVm>? Comments { get; set; }


}
