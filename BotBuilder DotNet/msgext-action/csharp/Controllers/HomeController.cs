using Microsoft.AspNetCore.Mvc;

namespace TeamsMessagingExtensionsAction.Controllers
{
    public class HomeController : Controller
    {
        [Route("/Home/RazorView")]
        public ActionResult RazorView()
        {
            return View("RazorView");
        }

        [Route("/Home/CustomForm")]
        public ActionResult CustomForm()
        {
            return View("CustomForm");
        }

        [Route("/Home/HtmlPage")]
        public ActionResult HtmlPage()
        {
            return View("HtmlPage");
        }
    }
}
