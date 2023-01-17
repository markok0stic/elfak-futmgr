using Microsoft.AspNetCore.Mvc;

namespace FutManager.Controllers
{
    public class LiveMatchController : Controller
    {
        public IActionResult Index(string channel)
        {
            ViewBag.Channel = channel;
            return View();
        }
    }
}
