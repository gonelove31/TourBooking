using BookingTour.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BookingTour.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("/admin/home/[action]/{id?}")]
    public class HomeController : Controller
    {
        private readonly TourContext _context;
        public HomeController(TourContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var tourCount = _context.tours.Count();

            ViewData["tourCount"] = tourCount;

            var userCount = _context.Users.Count();

            ViewData["userCount"] = userCount;

            var locaCount = _context.locations.Count();

            ViewData["locaCount"] = locaCount;

            var bookingCount = _context.bookings.Count();

            ViewData["bookings"] = bookingCount;

            // lấy ra số lượng thành công theo các tháng
            List<int> listSuccessCount = new List<int>();
            for (var i = 1; i <= 12; i++)
            {
                var successItem = _context.bookings.Where(b => b.BookingDate.Value.Month == i && b.Status == 1).Count();
                listSuccessCount.Add(successItem);
            }
            var successArrayJson = JsonConvert.SerializeObject(listSuccessCount);
            ViewData["successCount"] = successArrayJson;

            // lấy ra số lượng đã hủy theo các tháng
            List<int> listCancelCount = new List<int>();
            for (var i = 1; i <= 12; i++)
            {
                var cancelItem = _context.bookings.Where(b => b.BookingDate.Value.Month == i && b.Status == 2).Count();
                listCancelCount.Add(cancelItem);
            }
            var cancelArrayJson = JsonConvert.SerializeObject(listCancelCount);
            ViewData["cancelCount"] = cancelArrayJson;
            return View();
        }
    }
}
