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
using BookingTour.Services;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using Microsoft.AspNetCore.Identity;

namespace BookingTour.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("/admin/location/[action]/{id?}")]
    public class LocationsController : Controller
    {
        private readonly UserActionHistoryHelper _userActionHistoryHelper;
        private readonly TourContext _context;
        private readonly IWebHostEnvironment _evn;
        private readonly IViewRenderService _viewRenderService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SignInManager<AppUser> _signInManager;
        private bool IsAdminUser()
        {
            var isAdminClaim = User?.FindFirst("isAdmin")?.Value;
            return isAdminClaim == "true";
        }
        public LocationsController(TourContext context, IWebHostEnvironment evn, IViewRenderService viewRenderService, UserActionHistoryHelper userActionHistoryHelpe, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            _evn = evn;
            _context = context;
            _viewRenderService = viewRenderService;
            _userActionHistoryHelper = userActionHistoryHelpe;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
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
                await _userActionHistoryHelper.AddUserActionHistory("Create", "Thêm mới một Địa điểm trong danh sách Location có Location mới là: " + location.Name);
                return RedirectToAction(nameof(Index));
            }
            return View(location);
        }

        // GET: Admin/Locations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!IsAdminUser())
            {
                return Forbid(); // Hoặc chuyển hướng đến trang không có quyền
            }
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
            if (IsAdminUser())
            {
                return Forbid(); // Hoặc chuyển hướng đến trang không có quyền
            }
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
                    await _userActionHistoryHelper.AddUserActionHistory("Update", "cập nhật  một Địa điểm trong danh sách Location có Location mới là: " + location.Name);
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
            await _userActionHistoryHelper.AddUserActionHistory("Delete", "Xóa  một Địa điểm trong danh sách Location có Location cũ là: " + location.Name);
            return RedirectToAction(nameof(Index));
        }

        private bool LocationExists(int id)
        {
            return (_context.locations?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private IPagedList<Location> GetBookingData(int page = 1, int pageSize = 1000)
        {

            // Logic lấy dữ liệu Booking từ nguồn dữ liệu của bạn
            // Ví dụ: Lấy danh sách từ cơ sở dữ liệu

            var locations = _context.locations.ToPagedList(page, pageSize);

            return locations;
        }


        public async Task<IActionResult> ExportPdfLocation()
        {
         
           
            {
                // Render Partial View thành chuỗi HTML
                X.PagedList.IPagedList<Location> model = (X.PagedList.IPagedList<Location>)GetBookingData();
                string partialViewHtml = await _viewRenderService.RenderToStringAsync("partialViewLocation", model);

                // Tạo một tài liệu PDF từ chuỗi HTML
                using (MemoryStream stream = new MemoryStream())
                {
                    using (PdfWriter writer = new PdfWriter(stream))
                    {
                        using (PdfDocument pdf = new PdfDocument(writer))
                        {
                            HtmlConverter.ConvertToPdf(partialViewHtml, pdf, new ConverterProperties());
                        }
                    }

                    // Xuất file PDF với tên file là "ExportedPartialView.pdf"
                    byte[] pdfData = stream.ToArray();

                    // Thiết lập các headers HTTP tùy chỉnh nếu cần
                    Response.Headers.Add("Content-Disposition", "attachment; filename=Location.pdf");
                    Response.Headers.Add("Content-Type", "application/pdf");

                    // Trả về file PDF
                    return File(pdfData, "application/pdf");
                }



            }
        }
    }
}
