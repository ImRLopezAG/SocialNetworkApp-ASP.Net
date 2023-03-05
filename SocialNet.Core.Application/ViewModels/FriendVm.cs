using SocialNet.Core.Application.Core;
using System.ComponentModel.DataAnnotations;

namespace SocialNet.Core.Application.ViewModels;

public class FriendVm : BaseVm {
  public int SenderId { get; set; }
  public int ReceptorId { get; set; }
  // map ignore
  public ICollection<PostVm>? Posts { get; set; }
  public ICollection<UserVm>? Users { get; set; }

  [Required(ErrorMessage = "Username cannot be empty")]
  public string? UserName { get; set; }
  public string? UserImg { get; set; }
}
