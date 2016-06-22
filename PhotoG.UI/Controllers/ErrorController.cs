using System.Web.Mvc;

namespace PhotoG.UI.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/
        public ActionResult Index()
        {
            return View("Error");
        }
    }
}