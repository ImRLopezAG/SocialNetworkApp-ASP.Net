using AutoMapper;
using Microsoft.AspNetCore.Http;
using SocialNet.Core.Application.Contracts;
using SocialNet.Core.Application.Core;
using SocialNet.Core.Application.DTO;
using SocialNet.Core.Application.Helpers;
using SocialNet.Core.Application.Interfaces;
using SocialNet.Core.Application.ViewModels;
using SocialNet.Core.Application.ViewModels.SaveVm;
using SocialNet.Core.Domain.Entities;

namespace SocialNet.Core.Application.Services;

public class UserService : GenericService<UserVm, SaveUserVm, User>, IUserService {
  private readonly IUserRepository _userRepository;
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly IEmailService _emailService;

  private readonly IMapper _mapper;

  public UserService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, IEmailService emailService, IMapper mapper) : base(userRepository, mapper) {
    _userRepository = userRepository;
    _httpContextAccessor = httpContextAccessor;
    _emailService = emailService;
    _mapper = mapper;
  }


  public async Task<bool> UserExists(string UserName) => await _userRepository.Exists(us => us.UserName == UserName);

  public async Task<UserVm> Login(LoginVm vm) {
    ServiceResult result = new();
    try {
      var user = await _userRepository.Login(vm);
      if (user != null) {
        return _mapper.Map<UserVm>(user);
      } else {
        result.Success = false;
        result.Message = "User not found";
      }

    } catch {
      result.Success = false;
      result.Message = "Error while getting user";
    }
    return null;
  }

  public override async Task<SaveUserVm> Save(SaveUserVm vm) {
    try {
      var newUser = await base.Save(vm);
      await _emailService.SendEmail(new EmailRequest {
        To = newUser.Email,
        Subject = "Confirm your account",
        Body = EmailRequests.ConfirmEmail(newUser.Id, newUser.Name, newUser.LastName)
      });
      return _mapper.Map<SaveUserVm>(newUser);
    } catch {
      throw;
    }
  }
  public override async Task Edit(SaveUserVm vm) {
    try {
      var user = await _userRepository.GetEntity(vm.Id);
      if (user != null) {
        vm.Password = EncryptPassword.Encrypt(vm.Password);
        _mapper.Map(vm, user);
        await _userRepository.Update(user);
      }
    } catch {
      throw;
    }
  }

  public async Task ConfirmUser(int userId) {
    try {
      var user = await _userRepository.GetEntity(userId);
      if (user != null) {
        user.IsConfirmed = true;
        await _userRepository.Update(user);
      }
    } catch {
      throw;
    }
  }
  public async Task<UserVm> GetByUserName(string userName) {
    try {
      var user = await _userRepository.GetByUserName(userName);
      if (user != null) {
        return _mapper.Map<UserVm>(user);
      }
    } catch {
      throw;
    }
    return null;
  }

  public async Task ForgotPassword(string userName) {
    var user = await _userRepository.GetByUserName(userName);
    if (user != null) {
      var newPassword = Guid.NewGuid().ToString()[..8].Replace("-", "");
      user.Password = EncryptPassword.Encrypt(newPassword);
      await _userRepository.Update(user);
      await _emailService.SendEmail(new EmailRequest {
        To = user.Email,
        Subject = "New password",
        Body = EmailRequests.ResetPassword(user.UserName, newPassword)
      });
    }
  }
}
