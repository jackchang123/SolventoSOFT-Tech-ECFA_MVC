using ECFA_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using System.Text.Json; 

namespace ECFA_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly EcfaContext _context;
        public HomeController(ILogger<HomeController> logger, EcfaContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Route("Search")]
        public IActionResult Search()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [Route("Error/{statusCode?}")]
        [Route("Home/Error/{statusCode?}")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet("ServiceResponse")]
        public IActionResult ServiceResponse()
        {
            // 系統會自動去尋找 Views/Home/ServiceResponse.cshtml
            return View();
        }
        [HttpGet("sitemap")]
        public IActionResult Sitemap()
        {
            // 系統會自動去尋找 Views/Home/ServiceResponse.cshtml
            return View();
        }
    }
}
