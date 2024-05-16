using ediMvcApi.Models;
using ediMvcApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace ediMvcApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthModel _auth;
        private Response response = new Response();
        public AuthController(IConfiguration _config)
        {
            _auth = new AuthModel(_config);
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            int userId = HttpContext.Session.GetInt32("userId") ?? 0;
            if (userId != 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            User users =  _auth.LoginUser(user.email, user.password);
            if (users != null) 
            {
                HttpContext.Session.SetInt32("userId", (int)users.id);
                HttpContext.Session.SetInt32("userAkses", users.akses_id.GetValueOrDefault());
                HttpContext.Session.SetInt32("userBiodata", users.biodata_id.GetValueOrDefault());

                if (users.akses_id == 1)
                {
                    return RedirectToAction("Index", "AdminManage");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
                
            }
            return View();

        }

        public IActionResult Regist()
        {
            int userId = HttpContext.Session.GetInt32("userId") ?? 0;
            if (userId != 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            response = await _auth.CreateUser(user);
            if (response != null)
            {
                return Json(response);
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {

            HttpContext.Session.Clear();
            //HttpContext.Session.SetString("warnMsg", "Anda sudah keluar!");
            return RedirectToAction("Index", "Home");
        }
    }
}
