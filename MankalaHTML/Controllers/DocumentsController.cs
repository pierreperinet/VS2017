using System.Web.Mvc;

namespace MankalaHTML.Controllers
{
    public class DocumentsController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Documents";

            return View();
        }
    }
}
