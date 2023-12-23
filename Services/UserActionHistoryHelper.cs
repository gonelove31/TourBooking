using BookingTour.Models;
using Microsoft.AspNetCore.Identity;

namespace BookingTour.Services
{
    public class UserActionHistoryHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly UserManager<AppUser> _userManager;
       
        private readonly SignInManager<AppUser> _signInManager;
        private readonly TourContext _context;
        public UserActionHistoryHelper(UserManager<AppUser> userManager, TourContext context, SignInManager<AppUser> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task AddUserActionHistory(string action, string details)
        {
           
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            Console.WriteLine(_httpContextAccessor.HttpContext.User);
            if (user != null)
            {
                var userActionHistory = new UserActionHistory
                {
                    UserId = user.Id,
                    Action = action,
                    Timestamp = DateTime.Now,
                    Details = details
                };

                _context.UserActionHistories.Add(userActionHistory);
                await _context.SaveChangesAsync();
            }
            else
            {
                var userActionHistory = new UserActionHistory
                {
                    UserId = "5",
                    Action = action,
                    Timestamp = DateTime.Now,
                    Details = _httpContextAccessor.HttpContext.User.ToString()
                 
                };
                _context.UserActionHistories.Add(userActionHistory);
                await _context.SaveChangesAsync();
            }
            
        }
    }
}
