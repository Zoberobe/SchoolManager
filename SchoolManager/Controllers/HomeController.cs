using Microsoft.AspNetCore.Mvc;

namespace SchoolManager.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();//teste new 
        }
    }
}
