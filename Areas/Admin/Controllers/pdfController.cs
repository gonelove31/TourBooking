using BookingTour.Services;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.StyledXmlParser.Jsoup.Nodes;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using System.IO;
using iText.IO;
using iText;
using iText.Html2pdf;
using iText.Html2pdf.Resolver.Font;

using Microsoft.EntityFrameworkCore.Metadata.Internal;
using BookingTour.Models;
using X.PagedList;
using Microsoft.EntityFrameworkCore;

public class  PdfController : Controller
{
    private readonly TourContext _context;
    private readonly IViewRenderService _viewRenderService;

    public PdfController(IViewRenderService viewRenderService, TourContext context)
    {
        _viewRenderService = viewRenderService;
        _context = context;
    }


}
