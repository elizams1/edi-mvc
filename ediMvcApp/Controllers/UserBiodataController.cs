using Microsoft.AspNetCore.Mvc;

namespace ediMvcApp.Controllers
{
    public class UserBiodataController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
