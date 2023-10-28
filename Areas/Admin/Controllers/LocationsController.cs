using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookingTour.Models;
using BookingTour.Common;
using X.PagedList;

namespace BookingTour.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("/admin/location/[action]/{id?}")]
    public class LocationsController : Controller
    {
        private readonly TourContext _context;
        private readonly IWebHostEnvironment _evn;
        public LocationsController(TourContext context, IWebHostEnvironment evn)
        {
            _evn = evn;
            _context = context;
        }

        // GET: Admin/Locations
        public async Task<IActionResult> Index(int? page, string? searchString)
        {

            int pageSize = 5;
            int pageNumber = page ?? 1;
            var locations = from l in _context.locations select l;
            if (!String.IsNullOrEmpty(searchString))
            {
                locations = locations.Where(b => b.Name.Contains(searchString));
            }
            return View(locations.ToPagedList(pageNumber, pageSize));
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
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Slug, Image")] Location location, IFormFile file)
        {
            if (file != null)
            {
                var fileNamePath = new FileInfo(file.FileName);

                // lấy đường dẫn thư mục uploads gốc 
                var webPath = _evn.WebRootPath;
                var path = Path.Combine("", webPath + @"\uploads\" + fileNamePath);

                var pathToSave = @"/uploads/" + fileNamePath;

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                location.Image = pathToSave;
            }
            if (location.Slug == null) location.Slug = ConvertSlug.GenerateSlug(location.Name);
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
        public async Task<IActionResult> Edit(int id, [Bind("Name,Description,Slug, Image")] Location location, IFormFile file)
        {
            var locationEdit = (from c in _context.locations
                                where c.Id == id
                                select c).FirstOrDefault();

            if (file != null)
            {
                var fileNamePath = new FileInfo(file.FileName);

                // lấy đường dẫn thư mục uploads gốc 
                var webPath = _evn.WebRootPath;
                var path = Path.Combine("", webPath + @"\uploads\" + fileNamePath);

                var pathToSave = @"/uploads/" + fileNamePath;

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                location.Image = pathToSave;
            }
            else { location.Image = locationEdit.Image; }
            if (ModelState.IsValid)
            {
                try
                {
                    locationEdit.Name = location.Name;
                    locationEdit.Description = location.Description;
                    locationEdit.Slug = location.Slug;
                    locationEdit.ModifierDate = DateTime.Now;
                    locationEdit.Image = location.Image;
                    _context.Update(locationEdit);
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
