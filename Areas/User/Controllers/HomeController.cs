using BookingTour.Models;
using BookingTour.Services;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace BookingTour.Areas.User.Controllers
{
    [Area("User")]
    [Route("/bookingtour/home/[action]/{id?}")]
    public class HomeController : Controller
    {
        private readonly TourContext _context;
        private readonly IWebHostEnvironment _evn;
        public HomeController(TourContext context, IWebHostEnvironment evn )
        {
            _evn = evn;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
           var data = from t in _context.tours
                      select t;
            
            return View(data);
        }
    }
}
