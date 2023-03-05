using AutoMapper;
using MedicalManager.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using SocialNet.Core.Application.Contracts;
using SocialNet.Core.Application.Helpers;
using SocialNet.Core.Application.ViewModels;
using SocialNet.Core.Application.ViewModels.SaveVm;
using SocialNet.Web.Middleware;

namespace SocialNet.Presentation.Web.Controllers;
public class UserController : Controller {
  private readonly ValidateSessions _validateSessions;
  private readonly IUserService _userService;
  private readonly IHttpContextAccessor _httpContextAccessor;

  private readonly IMapper _mapper;

  public UserController(IUserService userService, IHttpContextAccessor httpContextAccessor, ValidateSessions validateSessions) {
    _userService = userService;
    _httpContextAccessor = httpContextAccessor;
    _validateSessions = validateSessions;
  }
  public IActionResult Index() {
    var url = Request.QueryString.Value;
    if (_validateSessions.HasUser())
      return RedirectToRoute(new { controller = "Home", action = "Index" });
    if (url.Contains("Error")) {
      ModelState.AddModelError("", "You need to be logged in to access this page");
      ViewBag.Message = "text-danger";
      return View();
    }
    if (url.Contains("Success")) {
      ModelState.AddModelError("", "Your account has been created, please check your email to confirm your account");
      ViewBag.Message = "text-success";
      return View();
    }
    if (url.Contains("Confirmed")) {
      ModelState.AddModelError("", "Your account has been confirmed, you can now log in");
      ViewBag.Message = "text-success";
      return View();
    }
    if (url.Contains("Restored")) {
      ModelState.AddModelError("", "Your password has been changed, please check your email to see your new password");
      ViewBag.Message = "text-success";
      return View();
    }

    return View();
  }



  [HttpPost]
  public async Task<IActionResult> Index(LoginVm model) {
    if (_validateSessions.HasUser())
      return RedirectToRoute(new { controller = "User", action = "Index" });
    try {
      if (!ModelState.IsValid)
        return View("Index", model);

      UserVm user = await _userService.Login(model);

      if (user != null) {
        if (user.IsConfirmed) {
          HttpContext.Session.Set<UserVm>("user", user);
          return RedirectToRoute(new { controller = "Home", action = "Index" });
        } else {
          ModelState.AddModelError("", "User not confirmed, please check your email and confirm your account");
          return View();
        }
      } else {
        ModelState.AddModelError("", "Invalid username or password");
        return View();
      }
    } catch {
      return View();
    }
  }

  public IActionResult LogOut() {
    HttpContext.Session.Remove("user");
    return RedirectToAction(nameof(Index));
  }

  public IActionResult Create() {
    if (_validateSessions.HasUser())
      return RedirectToRoute(new { controller = "Home", action = "Index" });

    return View("Create", new SaveUserVm { UserExists = false });
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Create(SaveUserVm model) {

    if (_validateSessions.HasUser())
      return RedirectToRoute(new { controller = "Home", action = "Index" });
    try {
      if (!ModelState.IsValid) return View("Create");

      if (_userService.UserExists(model.UserName).Result){
        return View("Create", new SaveUserVm { UserExists = true });
      }


      model.IsConfirmed = false;
      SaveUserVm user = await _userService.Save(model);

      if (user.Id != 0) {
        user.Image = ManageFile.Upload(model.ImageFile, user.Id);
        await _userService.Edit(user);
      }

      return RedirectToRoute(new { controller = "User", action = "Index", message = "Success" });
    } catch {
      return View();
    }
  }

  public async Task<IActionResult> Edit(int id) {
    if (!_validateSessions.HasUser())
      return RedirectToRoute(new { controller = "User", action = "Index", message = "Error" });

    var user = await _userService.GetEntity(id);
    if (( bool )!user.IsConfirmed)
      return RedirectToRoute(new { controller = "User", action = "Index" });

    return View("Edit", user);
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Edit(SaveUserVm model) {
    if (!_validateSessions.HasUser())
      return RedirectToRoute(new { controller = "User", action = "Index", message = "Error" });

    try {
      if (!ModelState.IsValid)
        return View("Edit", model);

      var user = await _userService.GetEntity(model.Id);
      if (user.UserName != model.UserName)
        if (_userService.UserExists(model.UserName).Result){
          return View("Edit", new SaveUserVm { UserExists = true });
        }

      model.Image = ManageFile.Upload(model.ImageFile, user.Id, true, user.Image);
      model.IsConfirmed = true;

      await _userService.Edit(model);

      var userSession = HttpContext.Session.Get<UserVm>("user");
      userSession.UserName = model.UserName;
      userSession.Image = model.Image;
      HttpContext.Session.Set<UserVm>("user", userSession);

      return RedirectToAction(nameof(Index));
    } catch {
      return View();
    }
  }

  public async Task<IActionResult> Confirm(int id) {
    try {
      var user = await _userService.GetEntity(id);
      if (user != null) {
        if (( bool )user.IsConfirmed)
          return RedirectToAction(nameof(Index));
        await _userService.ConfirmUser(id);

      }
    } catch {
      return RedirectToAction(nameof(Index));
    }

    return RedirectToRoute(new { controller = "User", action = "Index", message = "Confirmed" });
  }

  public IActionResult ForgotPassword() {
    if (_validateSessions.HasUser())
      return RedirectToRoute(new { controller = "Home", action = "Index" });

    return View();
  }

  [HttpPost]
  public async Task<IActionResult> ForgotPassword(string username) {
    if (_validateSessions.HasUser())
      return RedirectToRoute(new { controller = "Home", action = "Index" });

    if (string.IsNullOrEmpty(username)) {
      ModelState.AddModelError("", "the username is required");
      return View();
    }
    try {

      if (!_userService.UserExists(username).Result) {
        ModelState.AddModelError("", $"The user {username} doesn't exist.");
        return View();
      }

      await _userService.ForgotPassword(username);
      return RedirectToRoute(new { controller = "User", action = "Index", message = "Restored" });
    } catch {
      return View();
    }
  }
}

