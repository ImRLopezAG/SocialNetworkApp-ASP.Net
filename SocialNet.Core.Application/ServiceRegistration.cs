using Microsoft.Extensions.DependencyInjection;
using SocialNet.Core.Application.Contracts;
using SocialNet.Core.Application.Contracts.Core;
using SocialNet.Core.Application.Core;
using SocialNet.Core.Application.Services;
using System.Reflection;

namespace SocialNet.Core.Application;

public static class ServiceRegistration {
  public static void AddApplicationServices(this IServiceCollection services) {
    services.AddAutoMapper(Assembly.GetExecutingAssembly());

    #region Services
    services.AddTransient(typeof(IGenericService<,,>), typeof(GenericService<,,>));
    services.AddTransient<IUserService, UserService>();
    services.AddTransient<IPostService, PostService>();
    services.AddTransient<ICommentService, CommentService>();
    services.AddTransient<IFriendService, FriendService>();
    #endregion
  }
}
