
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
    }
}
