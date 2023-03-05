using MedicalManager.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using SocialNet.Core.Application.Contracts;
using SocialNet.Core.Application.Helpers;
using SocialNet.Core.Application.ViewModels;
using SocialNet.Core.Application.ViewModels.SaveVm;
using SocialNet.Web.Middleware;

namespace SocialNet.Presentation.Web.Controllers;

public class PostController : Controller {
  private readonly ValidateSessions _validateSessions;
  private readonly IPostService _postService;
  private readonly IHttpContextAccessor _httpContextAccessor;

  public PostController(IPostService postService, IHttpContextAccessor httpContextAccessor, ValidateSessions validateSessions) {
    _postService = postService;
    _httpContextAccessor = httpContextAccessor;
    _validateSessions = validateSessions;
  }
  private UserVm User => _httpContextAccessor.HttpContext.Session.Get<UserVm>("user");

  [HttpPost]
  public async Task<IActionResult> Create(HomeVm vm) {
    if (!_validateSessions.HasUser())
      return RedirectToRoute(new { controller = "User", action = "Index", message = "Error" });
    try {
      if (vm.ImageFile == null && vm.Content == null) {
        ModelState.AddModelError("", "You must write something or upload an image");
        return RedirectToRoute(new { controller = "Home", action = "Index" });
      }

      SavePostVm post = new() {
        Content = vm.Content,
        UserId = User.Id,
      };

      var newPost = await _postService.Save(post);
      if (newPost != null && vm.ImageFile != null) {
        newPost.Image = ManageFile.UploadPost(vm.ImageFile, User.Id);
        await _postService.Edit(newPost);
      }
      return RedirectToRoute(new { controller = "Home", action = "Index" });
    } catch {
      return View();
    }
  }

  public async Task<IActionResult> Edit(int id) {
    if (!_validateSessions.HasUser())
      return RedirectToRoute(new { controller = "User", action = "Index", message = "Error" });
    try {
      var post = await _postService.GetEntity(id);
      if (post.UserId != User.Id) {
        return RedirectToRoute(new { controller = "Home", action = "Index" });
      }
      if (post != null) {
        return View(post);
      }
      return RedirectToRoute(new { controller = "Home", action = "Index" });
    } catch {
      return View();
    }
  }



  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Edit(SavePostVm model) {
    if (!_validateSessions.HasUser())
      return RedirectToRoute(new { controller = "User", action = "Index", message = "Error" });
    try {
      var post = await _postService.GetEntity(model.Id);
      if (post != null) {
        post.Content = model.Content;
        post.Image = ManageFile.UploadPost(model.ImageFile, User.Id, true, post.Image);

        await _postService.Edit(post);
      }
      return RedirectToRoute(new { controller = "Home", action = "Index" });
    } catch {
      return View();
    }
  }
  public async Task<IActionResult> Delete(int id) {
    if (!_validateSessions.HasUser())
      return RedirectToRoute(new { controller = "User", action = "Index", message = "Error" });
    try {
      var post = await _postService.GetEntity(id);
      if (post.UserId != User.Id) {
        return RedirectToRoute(new { controller = "Home", action = "Index" });
      }

      await _postService.Delete(id);
      if (post.Image != null) {
        ManageFile.DeletePost(post.UserId, post.Image);
      }

      return RedirectToRoute(new { controller = "Home", action = "Index" });
    } catch {
      return View();
    }
  }
}


