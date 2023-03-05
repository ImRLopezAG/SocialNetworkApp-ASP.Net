using SocialNet.Core.Application.Contracts.Core;
using SocialNet.Core.Application.Core;
using SocialNet.Core.Application.ViewModels;
using SocialNet.Core.Application.ViewModels.SaveVm;
using SocialNet.Core.Domain.Entities;

namespace SocialNet.Core.Application.Contracts;

public interface IFriendService : IGenericService<FriendVm, SaveFriendVm, Friend> {
  Task SendRequest(int friendId);
  Task<ServiceResult> GetFriends();
  Task<ServiceResult> GetFriendsPosts();
  Task<bool> AreFriends(int friendId);
}
