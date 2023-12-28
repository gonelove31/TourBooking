using BookingTour.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace BookingTour.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("/Admin/UserActionHistories/[action]/{id?}")]
    public class UserActionHistories : Controller
    {
        private readonly TourContext _context;

        private readonly UserManager<AppUser> _userManager;

        public UserActionHistories(TourContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult UserActionHistory(string userId)
        {
            List<UserActionHistory> userActionHistories = _context.UserActionHistories.Include(history => history.User) .ToList();

            return View(userActionHistories);
        }
        public IActionResult DeleteHistory(int historyId)
        {
            var historyToRemove = _context.UserActionHistories.Find(historyId);
            if (historyToRemove != null)
            {
                _context.UserActionHistories.Remove(historyToRemove);
                _context.SaveChanges();
            }
            return RedirectToAction("UserActionHistory");
        }

        public IActionResult DeleteAllHistories()
        {
            var allHistory = _context.UserActionHistories.ToList();
            _context.UserActionHistories.RemoveRange(allHistory);
            _context.SaveChanges();
            return RedirectToAction("UserActionHistory");
        }
     
        public IActionResult SearchHistory(string username, string action, string date)
        {
            var query = _context.UserActionHistories.Include(history => history.User).AsQueryable();

            if (!string.IsNullOrEmpty(username))
            {
                query = query.Where(history => history.User.UserName.Contains(username));
            }

            if (!string.IsNullOrEmpty(action))
            {
                query = query.Where(history => history.Action.Contains(action));
            }

            if (!string.IsNullOrEmpty(date))
            {
                if (DateTime.TryParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
                {
                    query = query.Where(history => history.Timestamp.Date == parsedDate.Date);
                }
            }

            var foundHistory = query.ToList();
            return View("UserActionHistory", foundHistory);
        }
    }
}

