using System.Web.Mvc;

namespace PhotoG.UI.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            return View("Error");
        }
    }
}