using System.Web.Mvc;
using PhotoG.BL.Services;
using PhotoG.DAL.Entities;
using PhotoG.Infrastructure.Logging;

namespace PhotoG.UI.Controllers
{
    public class SearchController : Controller
    {
        private readonly IPhotoService _photoService;
        private readonly ILogger _logger;

        public SearchController(
            IPhotoService photoService,
            ILogger logger)
        {
            _photoService = photoService;
            _logger = logger;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(string title)
        {
            if (!ModelState.IsValid) return View("AdvancedSearchForm", title);

            _logger.Info("Trying to search photos with title='{0}'", title);
            var photos = _photoService.Search(title);

            return View("Result", photos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdvancedSearch(AdvancedSearchModel model)
        {
            if (!ModelState.IsValid) return View("AdvancedSearchForm", model);

            _logger.Info("Trying advanced search photos");
            var photos = _photoService.AdvancedSearch(model);

            return View("Result", photos);
        }
    }
}