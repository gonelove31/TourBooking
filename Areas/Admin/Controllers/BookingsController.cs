using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookingTour.Models;

namespace BookingTour.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("/admin/booking/[action]/{id?}")]
    public class BookingsController : Controller
    {
        private readonly TourContext _context;

        public BookingsController(TourContext context)
        {
            _context = context;
        }

        // GET: Admin/Bookings
        public async Task<IActionResult> Index()
        {
            var tourContext = _context.bookings.Include(b => b.Tour);
            return View(await tourContext.ToListAsync());
        }

        // GET: Admin/Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.bookings
                .Include(b => b.Tour)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Admin/Bookings/Create
        public IActionResult Create()
        {
            var tours = from t in _context.tours
                            select new
                            {
                                Value = t.Id,
                                Text = t.Id + " - " + t.Name,
                            };
            ViewData["TourID"] = new SelectList(tours.ToList(), "Value", "Text");
            return View();
        }

        // POST: Admin/Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustomerID,TourID,BookingDate,NumberOfPeople,TotalAmount")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                booking.CreatedBy = booking.ModifierBy = "Cuong";
                booking.CreatedDate = booking.ModifierDate = DateTime.Now;
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TourID"] = new SelectList(_context.tours, "Id", "Id", booking.TourID);
            return View(booking);
        }

        // GET: Admin/Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["TourID"] = new SelectList(_context.tours, "Id", "Id", booking.TourID);
            return View(booking);
        }

        // POST: Admin/Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerID,TourID,BookingDate,NumberOfPeople,TotalAmount,CreatedDate,CreatedBy,ModifierDate,ModifierBy")] Booking booking)
        {
            if (id != booking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TourID"] = new SelectList(_context.tours, "Id", "Id", booking.TourID);
            return View(booking);
        }

        // GET: Admin/Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.bookings
                .Include(b => b.Tour)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Admin/Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.bookings == null)
            {
                return Problem("Entity set 'TourContext.bookings'  is null.");
            }
            var booking = await _context.bookings.FindAsync(id);
            if (booking != null)
            {
                _context.bookings.Remove(booking);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
          return (_context.bookings?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
