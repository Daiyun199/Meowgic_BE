using Microsoft.AspNetCore.Mvc;

namespace Meowgic.API.Controllers
{
    public class ScheduleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
