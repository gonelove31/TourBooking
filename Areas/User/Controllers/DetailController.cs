
using BookingTour.Models;
using BookingTour.Services;
using iText.Commons.Actions.Contexts;
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

        [HttpGet]
        public IActionResult Booking(int? id) {
            var tour = (from t in _context.tours
                        where t.Id == id
                        select t).FirstOrDefault();
            ViewData["tour"] = tour;

            return View();
        }

        [HttpPost]
        public IActionResult Booking([Bind("CustomerName", "CustomerEmail", "CustomerPhone", "CustomerAddress, NumberOfAdult, NumberOfChildren")] Booking booking, int id)
        {
            var tour = (from b in _context.tours
                        where b.Id == id
                        select b).FirstOrDefault();
            booking.TourID = id;
            tour.AvailableSeats = tour.AvailableSeats - 1;
            if (ModelState.IsValid)
            {
                booking.Status = 1;
                booking.CustomerId = _userManager.GetUserId(User);
                booking.CreatedBy = booking.ModifierBy = "Cuong";
                booking.CreatedDate = booking.ModifierDate = DateTime.Now;
                booking.TotalAmount = booking.NumberOfAdult * tour.PriceAdult + booking.NumberOfChildren * tour.PriceChildren;
                booking.BookingDate = DateTime.Now;
                _context.bookings.Add(booking);
                _context.SaveChanges();
                return RedirectToAction(nameof(Cart));
            }
            else
            {
                ViewData["tour"] = tour;
                return View();
            }
        }
        [HttpPost("/user/detail/CancelBooking/{bookingId}")]
        public IActionResult CancelBooking(int bookingId)
        {
            try
            {
                // Lấy Booking từ cơ sở dữ liệu bằng ID
                var booking = _context.bookings.Find(bookingId);

                // Kiểm tra xem Booking có tồn tại không
                if (booking == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy Booking." });
                }

                // Cập nhật trạng thái thành 'Đã hủy'
                booking.Status = 2;
                var tour = (from b in _context.tours where b.Id==booking.TourID select b).FirstOrDefault();
                tour.AvailableSeats = tour.AvailableSeats + 1;
                // Lưu thay đổi vào cơ sở dữ liệu
                _context.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi:kk {ex.Message}" });
            }
        }


        [HttpGet]    
        public IActionResult Cart()
        {
            var userId = _userManager.GetUserId(User);
            var tourSuccess = _context.bookings.Where(b => b.CustomerId == userId).Include(t => t.Tour);
                //from t in _context.bookings
                //              where t.Status == 1 && t.CustomerId == userId
                //              select t;

            return View(tourSuccess);
        }


        public IActionResult Cart(int? id)
        {
            var tourSuccess = (from t in _context.bookings
                              where t.Id == id
                              select t).FirstOrDefault();
            tourSuccess.Status = 2;
            _context.bookings.Update(tourSuccess);
            _context.SaveChanges();
            return RedirectToAction("Cart");
        }
    }
}
