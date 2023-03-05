using Microsoft.AspNetCore.Mvc;
using SocialNet.Core.Application.Contracts;
using SocialNet.Core.Application.Helpers;
using SocialNet.Core.Application.ViewModels;
using SocialNet.Core.Application.ViewModels.SaveVm;
using SocialNet.Web.Middleware;

namespace SocialNet.Presentation.Web.Controllers;

public class CommentController : Controller {
  private readonly ValidateSessions _validateSessions;
  private readonly ICommentService _commentService;
  private readonly IHttpContextAccessor _httpContextAccessor;

  public CommentController(ICommentService commentService, IHttpContextAccessor httpContextAccessor, ValidateSessions validateSessions) {
    _commentService = commentService;
    _httpContextAccessor = httpContextAccessor;
    _validateSessions = validateSessions;
  }

  private UserVm User => _httpContextAccessor.HttpContext.Session.Get<UserVm>("user");

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Create(string comment, int postId) {
    if (!_validateSessions.HasUser())
      return RedirectToRoute(new { controller = "User", action = "Index", message = "Error" });
    try {
      var url = Request.Headers["Referer"].ToString();

      if (string.IsNullOrEmpty(comment)) {
        ModelState.AddModelError("", "Comment is required");
        return RedirectToAction(nameof(Index), url.Contains("/Friend") ? "Friend" : "Home");
      }
      SaveCommentVm commentToSave = new() {
        PostId = postId,
        UserId = User.Id,
        Content = comment
      };
      await _commentService.Save(commentToSave);
      return RedirectToAction(nameof(Index), url.Contains("/Friend") ? "Friend" : "Home");

    } catch {
      return View();
    }
  }
}

