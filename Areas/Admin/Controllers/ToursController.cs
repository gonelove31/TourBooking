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
    [Route("/admin/tours/[action]/{id?}")]
    public class ToursController : Controller
    {
        private readonly TourContext _context;

        public ToursController(TourContext context)
        {
            _context = context;
        }

        // GET: Admin/Tours
        public async Task<IActionResult> Index()
        {
            var tourContext = _context.tours.Include(t => t.Location);
            return View(await tourContext.ToListAsync());
        }

        // GET: Admin/Tours/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.tours == null)
            {
                return NotFound();
            }

            var tours = await _context.tours
                .Include(t => t.Location)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tours == null)
            {
                return NotFound();
            }

            return View(tours);
        }

        // GET: Admin/Tours/Create
        public IActionResult Create()
        {
            var locations = from l in _context.locations
                            select new
                            {
                                Value = l.Id,
                                Text = l.Id + " - " + l.Name,
                            };
            ViewData["LocationID"] = new SelectList(locations.ToList(), "Value", "Text");
            return View();
        }

        // POST: Admin/Tours/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Rate,Price,AvailableSeats,Slug,StartDate,EndDate,LocationID")] Tours tours)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tours);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocationID"] = new SelectList(_context.locations, "Id", "Id", tours.LocationID);
            return View(tours);
        }

        // GET: Admin/Tours/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            //if (id == null || _context.tours == null)
            //{
            //    return NotFound();
            //}

            var tours = await _context.tours.FindAsync(id);
            if (tours == null)
            {
                return NotFound();
            }
            var locations = from l in _context.locations
                            select new
                            {
                                Value = l.Id,
                                Text = l.Id + " - " + l.Name,
                            };
            ViewData["LocationID"] = new SelectList(locations.ToList(), "Value", "Text");
            return View(tours);
        }

        // POST: Admin/Tours/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Rate,Price,AvailableSeats,Slug,StartDate,EndDate,LocationID")] Tours tours)
        {
            if (id != tours.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tours);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToursExists(tours.Id))
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
            var locations = from l in _context.locations
                            select new
                            {
                                Value = l.Id,
                                Text = l.Id + " - " + l.Name,
                            };
            ViewData["LocationID"] = new SelectList(locations.ToList(), "Value", "Text");
            return View(tours);
        }

        // GET: Admin/Tours/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.tours == null)
            {
                return NotFound();
            }

            var tours = await _context.tours
                .Include(t => t.Location)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tours == null)
            {
                return NotFound();
            }

            return View(tours);
        }

        // POST: Admin/Tours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.tours == null)
            {
                return Problem("Entity set 'TourContext.tours'  is null.");
            }
            var tours = await _context.tours.FindAsync(id);
            if (tours != null)
            {
                _context.tours.Remove(tours);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ToursExists(int id)
        {
          return (_context.tours?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
