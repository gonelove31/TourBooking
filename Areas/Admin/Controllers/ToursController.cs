using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using BookingTour.Models;
using BookingTour.Common;
using X.PagedList;

namespace BookingTour.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("/Admin/Tours/[action]/{id?}")]
    public class ToursController : Controller
    {
        private readonly TourContext _context;
        private readonly IWebHostEnvironment _evn;
        public ToursController(TourContext context, IWebHostEnvironment evn)
        {
            _evn = evn;
            _context = context;
        }

        // GET: Admin/Tours
        public async Task<IActionResult> Index(int? page, string? searchString)
        {
            int pageSize = 5;
            int pageNumber = page ?? 1;
            IEnumerable<BookingTour.Models.Tours> tourContext = _context.tours.Include(t => t.Location).ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                tourContext = tourContext.Where(t => t.Name.Contains(searchString));
            }
            return View(tourContext.ToPagedList(pageNumber, pageSize));
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
            Console.WriteLine("fileName: ");
            var locations = from l in _context.locations
                            select new
                            {
                                Value = l.Id,
                                Text = l.Id + " - " + l.Name,
                            };
            ViewData["LocationID"] = new SelectList(locations.ToList(), "Value", "Text");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Image, Rate,PriceAdult, PriceChildren,AvailableSeats,Slug,StartDate,EndDate,LocationID")] Tours tours, IFormFile file)
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
                tours.Image = pathToSave;
            }
            if (tours.Slug == null) tours.Slug = ConvertSlug.GenerateSlug(tours.Name);
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
        public async Task<IActionResult> Edit(int id, [Bind("Name,Description,Image,Rate,PriceAdult,PriceChildren,AvailableSeats,Slug,StartDate,EndDate,LocationID")] Tours tours, IFormFile? file)
        {
            var tourEdit = (from c in _context.tours
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
                tours.Image = pathToSave;
            }
            else { tours.Image = tourEdit.Image; }
            if (ModelState.IsValid)
            {
                try
                {
                    tourEdit.Name = tours.Name;
                    tourEdit.Description = tours.Description;
                    tourEdit.Rate = tours.Rate;
                    tourEdit.PriceAdult = tours.PriceAdult;
                    tourEdit.PriceChildren = tours.PriceChildren;
                    tourEdit.AvailableSeats = tours.AvailableSeats;
                    tourEdit.Slug = tours.Slug;
                    tourEdit.StartDate = tours.StartDate;
                    tourEdit.EndDate = tours.EndDate;
                    tourEdit.LocationID = tours.LocationID;
                    tourEdit.Image = tours.Image;
                    _context.Update(tourEdit);
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
