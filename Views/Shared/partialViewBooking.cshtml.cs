using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BookingTour.Models;

namespace BookingTour.Views.Shared
{
    public class partialViewBookingModel : PageModel
    {
        private readonly BookingTour.Models.TourContext _context;

        public partialViewBookingModel(BookingTour.Models.TourContext context)
        {
            _context = context;
        }

        public IList<Booking> Booking { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.bookings != null)
            {
                Booking = await _context.bookings
                .Include(b => b.Tour).ToListAsync();
            }
        }
    }
}
