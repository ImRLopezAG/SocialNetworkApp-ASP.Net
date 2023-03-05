using SocialNet.Core.Application.Helpers;
using SocialNet.Core.Application.ViewModels;

namespace SocialNet.Web.Middleware;

public class ValidateSessions {
  private readonly IHttpContextAccessor _httpContextAccessor;

  public ValidateSessions(IHttpContextAccessor httpContextAccessor) {
    _httpContextAccessor = httpContextAccessor;
  }

  public bool HasUser() {
    UserVm userViewModel = _httpContextAccessor.HttpContext.Session.Get<UserVm>("user");
    return userViewModel != null;
  }
}
