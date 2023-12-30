using Microsoft.AspNetCore.Mvc;

namespace BookingTour.Areas.User.Controllers
{
    public class DetailController : Controller
    {
        [Area("User")]
        [Route("/bookingtour/detail/[action]/{id?}")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
