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
    public class partialViewTourModel : PageModel
    {
        private readonly BookingTour.Models.TourContext _context;

        public partialViewTourModel(BookingTour.Models.TourContext context)
        {
            _context = context;
        }

        public IList<Tours> Tours { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.tours != null)
            {
                Tours = await _context.tours
                .Include(t => t.Location).ToListAsync();
            }
        }
    }
}
