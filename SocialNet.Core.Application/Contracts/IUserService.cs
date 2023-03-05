using SocialNet.Core.Application.Contracts.Core;
using SocialNet.Core.Application.ViewModels;
using SocialNet.Core.Application.ViewModels.SaveVm;
using SocialNet.Core.Domain.Entities;

namespace SocialNet.Core.Application.Contracts;

public interface IUserService : IGenericService<UserVm, SaveUserVm, User> {
  Task<UserVm> Login(LoginVm vm);
  Task<bool> UserExists(string UserName);
  Task<UserVm> GetByUserName(string userName);

  Task ForgotPassword(string userName);
  Task ConfirmUser(int userId);
}
