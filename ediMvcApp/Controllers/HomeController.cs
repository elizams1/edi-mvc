using ediMvcApi.Models;
using ediMvcApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ediMvcApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserBiodataModel _userBiodataModel;

        public HomeController(ILogger<HomeController> logger, IConfiguration _config)
        {
            _logger = logger;
            _userBiodataModel = new UserBiodataModel(_config);
        }

        public IActionResult Index()
        {
            int userId = HttpContext.Session.GetInt32("userId")??0;
            int userBiodata = HttpContext.Session.GetInt32("userBiodata") ?? 0;
            if(userId != 0 && userBiodata !=0)
            {
                List<Biodata>? biodata = _userBiodataModel.GetBiodataById(userBiodata);
                return View(biodata);
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
