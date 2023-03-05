using Microsoft.EntityFrameworkCore;
using SocialNet.Core.Application.Helpers;
using SocialNet.Core.Application.Interfaces;
using SocialNet.Core.Application.ViewModels;
using SocialNet.Core.Domain.Entities;
using SocialNet.Infrastructure.Persistence.Context;
using SocialNet.Infrastructure.Persistence.Core;

namespace SocialNet.Infrastructure.Persistence.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository {
  private readonly SocialNetContext _context;
  public UserRepository(SocialNetContext context) : base(context) => _context = context;

  public async Task<User> GetByUserName(string userName) => await _context.Users.FirstOrDefaultAsync(x => x.UserName == userName && x.IsConfirmed == true);

  public async Task<User> Login(LoginVm login) {
    string passwordHash = EncryptPassword.Encrypt(login.Password);
    return await _context.Users.FirstOrDefaultAsync(x => x.UserName == login.UserName && x.Password == passwordHash);
  }
}