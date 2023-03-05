using SocialNet.Core.Application.Core;
using SocialNet.Core.Domain.Entities;

namespace SocialNet.Core.Application.Interfaces;

public interface IFriendRepository : IGenericRepository<Friend> {
  Task<bool> AreFriends(int userId, int friendId);

}
