using SocialNet.Core.Application.Core;
using SocialNet.Core.Application.ViewModels;
using SocialNet.Core.Domain.Entities;

namespace SocialNet.Core.Application.Interfaces;

public interface IUserRepository : IGenericRepository<User> {
  Task<User> Login(LoginVm login);

  Task<User> GetByUserName(string userName);
}
