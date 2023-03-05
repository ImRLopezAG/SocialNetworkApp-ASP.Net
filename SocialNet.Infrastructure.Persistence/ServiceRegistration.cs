using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SocialNet.Core.Application.Core;
using SocialNet.Core.Application.Interfaces;
using SocialNet.Infrastructure.Persistence.Context;
using SocialNet.Infrastructure.Persistence.Core;
using SocialNet.Infrastructure.Persistence.Repositories;

namespace SocialNet.Infrastructure.Persistence {
  public static class ServiceRegistration {
    public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration) {
      #region DbContext
      if (configuration.GetValue<bool>("UseInMemoryDatabase")) {
        services.AddDbContext<SocialNetContext>(options => options.UseInMemoryDatabase("ApplicationDb"));
      } else {
        services.AddDbContext<SocialNetContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
        m => m.MigrationsAssembly(typeof(SocialNetContext).Assembly.FullName)));
      }
      #endregion
      #region Repositories
      services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
      services.AddScoped<IUserRepository, UserRepository>();
      services.AddScoped<IPostRepository, PostRepository>();
      services.AddScoped<ICommentRepository, CommentRepository>();
      services.AddScoped<IFriendRepository, FriendRepository>();
      #endregion
    }
  }
}