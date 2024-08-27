using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NET_FRAMEWORKS_EXAMEN_OPDRACHT.Models;
using NET_FRAMEWORKS_EXAMEN_OPDRACHT.Areas.Identity.Data;

namespace NET_FRAMEWORKS_EXAMEN_OPDRACHT.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger; // Logger for logging information
        private readonly SignInManager<GarageUser> _signInManager; // Manager to handle sign-in operations

        // Constructor to inject logger and SignInManager
        public HomeController(ILogger<HomeController> logger, SignInManager<GarageUser> signInManager)
        {
            _logger = logger;
            _signInManager = signInManager;
        }

        // GET: Home/Index
        public IActionResult Index()
        {
            // Check if the current user is signed in
            if (_signInManager.IsSignedIn(User))
            {
                // Retrieve a flag indicating if a cookie is present
                bool cookiePresente = (bool)HttpContext.Items["CookiePresente"];
                // You may want to use the cookiePresente value here
            }
            else
            {
                // Redirect to the login page if the user is not signed in
                return Redirect("/Identity/Account/Login");
                // Alternative URL formats for the login page are noted in comments
            }

            // Return the Index view if the user is signed in
            return View();
        }

        // GET: Home/Privacy
        public IActionResult Privacy()
        {
            return View(); // Return the Privacy view
        }

        // GET: Home/Error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Return the Error view with the request ID or trace identifier for debugging
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
