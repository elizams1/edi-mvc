using ediMvcApi.Models;
using ediMvcApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ediMvcApp.Controllers
{

    public class AdminManageController : Controller
    {

        private readonly AdminManageModel _admin;
        private Response response = new Response();
        public AdminManageController(IConfiguration _config)
        {
            _admin = new AdminManageModel(_config);
        }
        public IActionResult Index(string? filter)
        {
            int userAkses = HttpContext.Session.GetInt32("userAkses") ?? 0;
            if(userAkses != 1)
            {
                return RedirectToAction("Index", "Home");
            }

            List<Biodata>? biodata = new List<Biodata>();
            if (filter == null)
            {
                biodata = _admin.GetAllBiodata();
            }
            else
            {
                biodata = _admin.GetBiodataByString(filter);
            }
            
            return View(biodata);
        }
    }
}
