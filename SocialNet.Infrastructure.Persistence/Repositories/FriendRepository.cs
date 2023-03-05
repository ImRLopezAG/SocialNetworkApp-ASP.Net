using Microsoft.EntityFrameworkCore;
using SocialNet.Core.Application.Interfaces;
using SocialNet.Core.Domain.Entities;
using SocialNet.Infrastructure.Persistence.Context;
using SocialNet.Infrastructure.Persistence.Core;

namespace SocialNet.Infrastructure.Persistence.Repositories;

public class FriendRepository : GenericRepository<Friend>, IFriendRepository {
  private readonly SocialNetContext _context;
  private readonly IUserRepository _userRepository;
  public FriendRepository(SocialNetContext context, IUserRepository userRepository) : base(context) {
    _context = context;
    _userRepository = userRepository;
  }

  public async Task<bool> AreFriends(int userId, int friendId) {
    var friend = await _context.Friends.FirstOrDefaultAsync(f => f.SenderId == userId && f.ReceptorId == friendId || f.SenderId == friendId && f.ReceptorId == userId);
    return friend != null;
  }

}
