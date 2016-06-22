using System;
using System.Web.Mvc;
using PhotoG.BL.Services;
using PhotoG.Infrastructure.Logging;

namespace PhotoG.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAlbumService _albumService;

        public HomeController(IAlbumService albumService)
        {
            _albumService = albumService;
        }

        public ActionResult Index()
        {
            var albums = _albumService.GetAlbums();

            return View(albums);
        }
    }
}