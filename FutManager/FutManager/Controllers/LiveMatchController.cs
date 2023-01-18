using Microsoft.AspNetCore.Mvc;

namespace FutManager.Controllers
{
    public class LiveMatchController : Controller
    {
        public IActionResult Index(string channel)
        {
            ViewData["Title"] = "Live";
            ViewBag.Channel = channel;
            return View();
        }
    }
}
