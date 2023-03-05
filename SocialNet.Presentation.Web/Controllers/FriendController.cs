using Microsoft.AspNetCore.Mvc;
using SocialNet.Core.Application.Contracts;
using SocialNet.Core.Application.Helpers;
using SocialNet.Core.Application.ViewModels;
using SocialNet.Web.Middleware;

namespace SocialNet.Presentation.Web.Controllers;

public class FriendController : Controller {
  private readonly ValidateSessions _validateSessions;

  private readonly IFriendService _friendService;
  private readonly IPostService _postService;
  private readonly IUserService _userService;
  private readonly IHttpContextAccessor _httpContextAccessor;


  public FriendController(IFriendService friendService, IHttpContextAccessor httpContextAccessor, IPostService postService, IUserService userService, ValidateSessions validateSessions) {
    _friendService = friendService;
    _httpContextAccessor = httpContextAccessor;
    _validateSessions = validateSessions;
    _postService = postService;
    _userService = userService;
  }

  private UserVm User => _httpContextAccessor.HttpContext.Session.Get<UserVm>("user");

  public async Task<IActionResult> Index() {
    if (!_validateSessions.HasUser())
      return RedirectToRoute(new { controller = "User", action = "Index", message = "Error" });
    try {

      var posts = await _friendService.GetFriendsPosts().ContinueWith(t => t.Result.Data);
      var friends = await _friendService.GetFriends().ContinueWith(t => t.Result.Data);
      return View("Index", new FriendVm {
        Posts = ( List<PostVm> )posts,
        Users = ( List<UserVm> )friends
      });
    } catch {
      return View();
    }
  }


  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> AddFriend(string username) {
    if (!_validateSessions.HasUser())
      return RedirectToRoute(new { controller = "User", action = "Index", message = "Error" });
    try {
      var user = await _userService.GetByUserName(username);
      if (user != null) {
        if (user.Id == User.Id) {
          ModelState.AddModelError("", "You can not add your self");
          return RedirectToAction(nameof(Index));
        }

        if (_friendService.AreFriends(user.Id).Result)
          ModelState.AddModelError("", $"The User {username} is already your friend");
        else
          await _friendService.SendRequest(user.Id);
      } else {
        ModelState.AddModelError("", $"The User {username} not found");
      }

      return RedirectToAction(nameof(Index));
    } catch {
      return View("Index");
    }
  }

  public async Task<IActionResult> Delete(int id) {
    if (!_validateSessions.HasUser())
      return RedirectToRoute(new { controller = "User", action = "Index", message = "Error" });
    try {
      await _friendService.Delete(id);
      return RedirectToAction(nameof(Index));
    } catch {
      return View();
    }
  }
}

