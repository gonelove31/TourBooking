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
    [Route("/admin/location/[action]/{id?}")]
    public class LocationsController : Controller
    {
        private readonly TourContext _context;

        public LocationsController(TourContext context)
        {
            _context = context;
        }

        // GET: Admin/Locations
        public async Task<IActionResult> Index()
        {
              return _context.locations != null ? 
                          View(await _context.locations.ToListAsync()) :
                          Problem("Entity set 'TourContext.locations'  is null.");
        }

        // GET: Admin/Locations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.locations == null)
            {
                return NotFound();
            }

            var location = await _context.locations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // GET: Admin/Locations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Locations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Slug")] Location location)
        {
            if (ModelState.IsValid)
            {
                location.CreatedBy = location.ModifierBy = "Cuong";
                location.CreatedDate = location.ModifierDate = DateTime.Now;
                _context.Add(location);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(location);
        }

        // GET: Admin/Locations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.locations == null)
            {
                return NotFound();
            }

            var location = await _context.locations.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }
            return View(location);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Description,Slug")] Location location)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var location_old = (from l in _context.locations where l.Id == id select l).FirstOrDefault();
                    location_old.Name = location.Name;
                    location_old.Description = location.Description;
                    location_old.Slug = location.Slug;
                    location_old.ModifierDate = DateTime.Now;
                    _context.Update(location_old);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    
                }
                return RedirectToAction(nameof(Index));
            }
            return View(location);
        }

        // GET: Admin/Locations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.locations == null)
            {
                return NotFound();
            }

            var location = await _context.locations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // POST: Admin/Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.locations == null)
            {
                return Problem("Entity set 'TourContext.locations'  is null.");
            }
            var location = await _context.locations.FindAsync(id);
            if (location != null)
            {
                _context.locations.Remove(location);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocationExists(int id)
        {
          return (_context.locations?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
