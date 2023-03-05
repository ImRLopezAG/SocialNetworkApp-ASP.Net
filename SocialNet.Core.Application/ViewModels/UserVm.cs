using SocialNet.Core.Application.ViewModels.SaveVm;

namespace SocialNet.Core.Application.ViewModels;

public class UserVm : BasePersonVm {
  public string UserName { get; set; } = null!;
  public string Password { get; set; } = null!;
  public string? Image { get; set; }

  public bool IsConfirmed { get; set; }

  // ignore mapping
  public ICollection<PostVm> Posts { get; set; } = null!;
  public ICollection<CommentVm> Comments { get; set; } = null!;
  public ICollection<FriendVm> Friends { get; set; } = null!;

  public static implicit operator UserVm(SaveUserVm v) {
    throw new NotImplementedException();
  }
}
