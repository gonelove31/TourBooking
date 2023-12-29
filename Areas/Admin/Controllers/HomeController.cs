using BookingTour.Models;
using iText.Commons.Actions.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            // lấy top 5 tour được đặt nhiều nhất 
            var top5BookedTours = (
            from tour in _context.tours
            join booking in _context.bookings on tour.Id equals booking.TourID
            group tour by new
            {
                tour.Id,
                tour.Name,
                tour.Description,
                tour.Rate,
                tour.PriceAdult,
                tour.PriceChildren,
                tour.AvailableSeats,
                tour.Slug,
                tour.StartDate,
                tour.EndDate,
                tour.LocationID,
                tour.Image
            } into tourGroup
            orderby tourGroup.Count() descending
            select new  // Chuyển đổi thành kiểu Tours
            {
                Id = tourGroup.Key.Id,
                Name = tourGroup.Key.Name,
                Description = tourGroup.Key.Description,
                Rate = tourGroup.Key.Rate,
                PriceAdult = tourGroup.Key.PriceAdult,
                PriceChildren = tourGroup.Key.PriceChildren,
                AvailableSeats = tourGroup.Key.AvailableSeats,
                Slug = tourGroup.Key.Slug,
                StartDate = tourGroup.Key.StartDate,
                EndDate = tourGroup.Key.EndDate,
                LocationID = tourGroup.Key.LocationID,
                Image = tourGroup.Key.Image,
                countBooking = tourGroup.Count()
            }
        ).Take(5).ToList();


            return View(top5BookedTours);
        }
    }
}
