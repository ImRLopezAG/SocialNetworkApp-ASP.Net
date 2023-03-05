using SocialNet.Core.Application.Core;

namespace SocialNet.Core.Application.ViewModels.SaveVm;

public class SaveFriendVm : BaseVm {
  public int SenderId { get; set; }
  public int ReceptorId { get; set; }
}
