using ContractMonthlyClaimsSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ContractMonthlyClaimsSystem.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult LecturerHome()
        {
            HttpContext.Session.SetString("Role", "Lecturer");
            return RedirectToAction("Home", "Lecturer");
        }

        public IActionResult ReviewerHome()
        {
            HttpContext.Session.SetString("Role", "Reviewer");
            return RedirectToAction("Home", "Reviewer");
        }

        public IActionResult HrHome()
        {
            HttpContext.Session.SetString("Role", "HR");
            return RedirectToAction("Home", "HR");
        }

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
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
