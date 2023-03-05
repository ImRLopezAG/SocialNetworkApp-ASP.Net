using Microsoft.AspNetCore.Mvc;
using SocialNet.Core.Application.Contracts;
using SocialNet.Core.Application.ViewModels;
using SocialNet.Presentation.Web.Models;
using SocialNet.Web.Middleware;
using System.Diagnostics;

namespace SocialNet.Presentation.Web.Controllers {
  public class HomeController : Controller {
    private readonly ValidateSessions _validateSessions;
    private readonly ILogger<HomeController> _logger;
    private readonly IPostService _postService;

    public HomeController(ILogger<HomeController> logger, IPostService postService, ValidateSessions validateSessions) {
      _logger = logger;
      _postService = postService;
      _validateSessions = validateSessions;
    }


    public async Task<IActionResult> Index() {
      if (!_validateSessions.HasUser())
        return RedirectToRoute(new { controller = "User", action = "Index", message = "Error" });

      var posts = await _postService.GetAll().ContinueWith(t => t.Result.Data);
      return View("Index", new HomeVm { Posts = ( List<PostVm> )posts });
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}