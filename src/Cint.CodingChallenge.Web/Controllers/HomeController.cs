using Microsoft.AspNetCore.Mvc;

namespace Cint.CodingChallenge.Web.Controllers
{
    public class HomeController : Controller
    {
        // Because of attribute routing I can't change the default route so I'm redirecting to the search page...
        public IActionResult Index()
        {
            return RedirectToAction("search","survey");
        }
    }
}