using Microsoft.AspNetCore.Mvc;

namespace FutManager.Controllers
{
    public class LiveMatchController : Controller
    {
        public IActionResult Index(string channel)
        {
            ViewData["Title"] = "Live";
            return View(channel);
        }
    }
}
