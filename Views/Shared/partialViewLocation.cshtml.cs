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
    public class partialViewLocationModel : PageModel
    {
        private readonly BookingTour.Models.TourContext _context;

        public partialViewLocationModel(BookingTour.Models.TourContext context)
        {
            _context = context;
        }

        public IList<Location> Location { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.locations != null)
            {
                Location = await _context.locations.ToListAsync();
            }
        }
    }
}
