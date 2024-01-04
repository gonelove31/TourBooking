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
     
        public IActionResult SearchHistory([Bind("UserName", "Action")] UserActionHistory us)
        {
            var query = from b in _context.UserActionHistories.Include(history => history.User) select b;
            if (!string.IsNullOrEmpty(us.UserName))
            {
               query =from b in _context.UserActionHistories.Include(history => history.User) where b.UserName.Contains(us.UserName) select b;
            }

            if (!string.IsNullOrEmpty(us.Action))
            {
                query = from b in _context.UserActionHistories.Include(history => history.User) where b.Action.Contains(us.Action) select b;

            }

         

            var foundHistory = query.ToList();
            return View("UserActionHistory", foundHistory);
        }
    }
}

