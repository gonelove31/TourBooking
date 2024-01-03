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
    [Route("/admin/image/[action]/{id?}")]
    public class ImageToursController : Controller
    {
        private readonly TourContext _context;
        private readonly IWebHostEnvironment _evn;
        public ImageToursController(TourContext context, IWebHostEnvironment evn)
        {
            _context = context;
            _evn = evn;
        }

        // GET: Admin/ImageTours
        public async Task<IActionResult> Index(string? searchString)
        {
            IEnumerable<BookingTour.Models.ImageTour> tourContext = _context.image.Include(i => i.Tours).ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                tourContext = tourContext.Where(t => t.Tours.Name.Contains(searchString));
            }
            return View(tourContext);
        }

        // GET: Admin/ImageTours/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.image == null)
            {
                return NotFound();
            }

            var imageTour = await _context.image
                .Include(i => i.Tours)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (imageTour == null)
            {
                return NotFound();
            }

            return View(imageTour);
        }

        // GET: Admin/ImageTours/Create
        public IActionResult Create()
        {
            ViewData["TourID"] = new SelectList(_context.tours, "Id", "Name");
            return View();
        }

        // POST: Admin/ImageTours/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,URL,TourID")] ImageTour imageTour, IFormFile file)
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
                imageTour.URL = pathToSave;
            }
            if (ModelState.IsValid)
            {
                _context.Add(imageTour);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TourID"] = new SelectList(_context.tours, "Id", "Name", imageTour.TourID);
            return View(imageTour);
        }

        // GET: Admin/ImageTours/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.image == null)
            {
                return NotFound();
            }

            var imageTour = await _context.image.FindAsync(id);
            if (imageTour == null)
            {
                return NotFound();
            }
            ViewData["TourID"] = new SelectList(_context.tours, "Id", "Name", imageTour.TourID);
            return View(imageTour);
        }

        // POST: Admin/ImageTours/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,URL,TourID")] ImageTour imageTour, IFormFile file)
        {

            if (id != imageTour.Id)
            {
                return NotFound();
            }
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
                imageTour.URL = pathToSave;
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(imageTour);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImageTourExists(imageTour.Id))
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
            ViewData["TourID"] = new SelectList(_context.tours, "Id", "Name", imageTour.TourID);
            return View(imageTour);
        }

        // GET: Admin/ImageTours/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.image == null)
            {
                return NotFound();
            }

            var imageTour = await _context.image
                .Include(i => i.Tours)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (imageTour == null)
            {
                return NotFound();
            }

            return View(imageTour);
        }

        // POST: Admin/ImageTours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.image == null)
            {
                return Problem("Entity set 'TourContext.image'  is null.");
            }
            var imageTour = await _context.image.FindAsync(id);
            if (imageTour != null)
            {
                _context.image.Remove(imageTour);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImageTourExists(int id)
        {
          return (_context.image?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
