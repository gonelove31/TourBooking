using Microsoft.AspNetCore.Mvc;

namespace BookingTour.Areas.User.Controllers
{
    [Area("User")]
    [Route("/bookingtour/home/[action]/{id?}")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
