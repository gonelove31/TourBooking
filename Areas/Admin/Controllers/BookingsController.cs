using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookingTour.Models;
using X.PagedList;
using BookingTour.Services;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using Microsoft.AspNetCore.Identity;
using iText.Kernel.Geom;

namespace BookingTour.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("/admin/booking/[action]/{id?}")]
    public class BookingsController : Controller
    {
        private readonly UserActionHistoryHelper _userActionHistoryHelper;
        private readonly TourContext _context;
        private readonly IViewRenderService _viewRenderService;
        private readonly UserManager<AppUser> _userManager;
        public BookingsController(IViewRenderService viewRenderService, TourContext context, UserManager<AppUser> userManager, UserActionHistoryHelper userActionHistoryHelper)
        {
            // Các thiết lập khác nếu cần
            _viewRenderService = viewRenderService;
            _context = context;
            _userManager = userManager;
            _userActionHistoryHelper = userActionHistoryHelper;
        }

        // GET: Admin/Bookings
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;
            var tourContext = _context.bookings.Include(b => b.Tour);

            return View(tourContext.ToPagedList(pageNumber, pageSize));


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

            var status = new List<Object>(){
                new {
                    Value = 1,
                    Text = "Thành công"
                },
                new {
                    Value = 2,
                    Text = "Đã hủy"
                },
            };
            ViewData["StatusID"] = new SelectList(status.ToList(), "Value", "Text");
            return View();
        }

        // POST: Admin/Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustomerID,TourID,BookingDate,NumberOfAdult, NumberOfChildren,TotalAmount, Status")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                booking.CreatedBy = booking.ModifierBy = "Cuong";
                booking.CreatedDate = booking.ModifierDate = DateTime.Now;
                _context.Add(booking);
                await _context.SaveChangesAsync();
                //fff
                await _userActionHistoryHelper.AddUserActionHistory("Create", "Thêm mới một Booking trong danh sách Booking có mã là: "+ booking.Id);
                return RedirectToAction(nameof(Index));
            }
            ViewData["TourID"] = new SelectList(_context.tours, "Id", "Id", booking.TourID);
            var status = new List<Object>(){
                new {
                    Value = 1,
                    Text = "Thành công"
                },
                new {
                    Value = 2,
                    Text = "Đã hủy"
                },
            };
           
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
            var status = new List<Object>(){
                new {
                    Value = 1,
                    Text = "Thành công"
                },
                new {
                    Value = 2,
                    Text = "Đã hủy"
                },
            };
            ViewData["StatusID"] = new SelectList(status.ToList(), "Value", "Text");
            return View(booking);
        }

        // POST: Admin/Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerID,TourID,BookingDate,NumberOfChildren,NumberOfAdult,TotalAmount, Status")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var bookingEdit = (from b in _context.bookings
                                       where b.Id == id
                                       select b).FirstOrDefault();

                    bookingEdit.CustomerID = booking.CustomerID;
                    bookingEdit.TourID = booking.TourID;
                    bookingEdit.BookingDate = booking.BookingDate;
                    bookingEdit.NumberOfChildren = booking.NumberOfChildren;
                    bookingEdit.NumberOfAdult = booking.NumberOfAdult;
                    bookingEdit.TotalAmount = booking.TotalAmount;
                    bookingEdit.Status = booking.Status;
                    bookingEdit.ModifierDate = DateTime.Now;
                    bookingEdit.ModifierBy = "Cuong";
                    _context.Update(bookingEdit);
                    await _context.SaveChangesAsync();
                    await _userActionHistoryHelper.AddUserActionHistory("Update", "cập nhật  một Booking trong danh sách Booking có CustomerID mới là: " + booking.CustomerID + " và TourID mới là" + booking.TourID);
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
            var status = new List<Object>(){
                new {
                    Value = 1,
                    Text = "Thành công"
                },
                new {
                    Value = 2,
                    Text = "Đã hủy"
                },
            };
            ViewData["StatusID"] = new SelectList(status.ToList(), "Value", "Text");
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
            await _userActionHistoryHelper.AddUserActionHistory("Delete", "Xóa  một Booking trong danh sách Booking có CustomerID la: " + booking.CustomerID + " và TourID  là: " + booking.TourID);
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return (_context.bookings?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private IPagedList<Booking> GetBookingData(int page = 1, int pageSize = 1000)
        {

            // Logic lấy dữ liệu Booking từ nguồn dữ liệu của bạn
            // Ví dụ: Lấy danh sách từ cơ sở dữ liệu

            var bookings = _context.bookings.ToPagedList(page, pageSize);

            return bookings;
        }


        public async Task<IActionResult> ExportPdfBooking()
        {
            
            // Render Partial View thành chuỗi HTML
            X.PagedList.IPagedList<Booking> model = (X.PagedList.IPagedList<Booking>)GetBookingData();
            string partialViewHtml = await _viewRenderService.RenderToStringAsync("partialViewBooking", model);

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
                Response.Headers.Add("Content-Disposition", "attachment; filename=Booking.pdf");
                Response.Headers.Add("Content-Type", "application/pdf");

                // Trả về file PDF
                return File(pdfData, "application/pdf");
            }


        }
    }
}
