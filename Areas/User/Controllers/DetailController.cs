
using BookingTour.Models;
using BookingTour.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingTour.Areas.User.Controllers
{
    [Area("User")]
    [Route("/user/detail/[action]/{id?}")]

    public class DetailController : Controller
    {
        private readonly UserActionHistoryHelper _userActionHistoryHelper;
        private readonly TourContext _context;
        private readonly IViewRenderService _viewRenderService;
        private readonly UserManager<AppUser> _userManager;
        public DetailController(IViewRenderService viewRenderService, TourContext context, UserManager<AppUser> userManager, UserActionHistoryHelper userActionHistoryHelper)
        {
            // Các thiết lập khác nếu cần
            _viewRenderService = viewRenderService;
            _context = context;
            _userManager = userManager;
            _userActionHistoryHelper = userActionHistoryHelper;
        }
    
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null || _context.tours == null)
            {
                return NotFound();
            }

            var tour = await _context.tours
                .Include(t => t.Location)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tour == null)
            {
                return NotFound();

            }

            return View(tour);
        }
        public IActionResult Booking(int? id) {
            var tour = (from t in _context.tours
                        where t.Id == id
                        select t).FirstOrDefault();
            ViewData["tour"] = tour;

            return View();
        }

        public IActionResult confirmBooking([Bind("CustomerName", "CustomerEmail", "CustomerPhone", "CustomerAddress, NumberOfAdult, NumberOfChildren")] Booking booking ,int id)
        {
            booking.Status = 1;
            booking.CustomerId = _userManager.GetUserId(User);
            booking.TourID = id;

            
            
                var tour = (from b in _context.tours
                            where b.Id == id
                            select b).FirstOrDefault();
                booking.CreatedBy = booking.ModifierBy = "Cuong";
                booking.CreatedDate = booking.ModifierDate = DateTime.Now;
                booking.TotalAmount = booking.NumberOfAdult * tour.PriceAdult + booking.NumberOfChildren * tour.PriceChildren;
                booking.BookingDate = DateTime.Now;
                _context.bookings.Add(booking);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            
        }
    }
}
