using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PhotoG.BL.Services;
using PhotoG.Infrastructure.Identity;
using PhotoG.Infrastructure.Identity.Managers;

namespace PhotoG.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppUserManager _userManager;
        private readonly IAlbumService _albumService;

        public HomeController(
            AppUserManager userManager, 
            IAlbumService albumService)
        {
            _userManager = userManager;
            _albumService = albumService;
        }

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.IsBaseUser = _userManager.IsInRole(User.Identity.GetUserId(), Roles.Base.ToString());
            }

            var albums = _albumService.GetAlbums();

            return View(albums);
        }
    }
}