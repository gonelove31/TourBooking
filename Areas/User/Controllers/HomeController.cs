using BookingTour.Models;
using BookingTour.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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
       
        public async Task<IActionResult> Index(string? searchString)
        {
            IEnumerable<BookingTour.Models.Tours> tourContext = _context.tours.Include(t => t.Location).ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                tourContext = tourContext.Where(t => t.Name.Contains(searchString));
            }
            return View(tourContext.ToPagedList());
        }
    }
}
