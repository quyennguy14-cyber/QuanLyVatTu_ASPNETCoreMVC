
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyVatTu.Data;

namespace QuanLyVatTu.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _db;
    private readonly ILogger<HomeController> _logger;

    public HomeController(
        AppDbContext db,
        ILogger<HomeController> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            ViewBag.MaterialCount = await _db.Materials.CountAsync();

            ViewBag.LowStock = await _db.Materials.CountAsync(
                x => x.Quantity <= x.MinStock);

            ViewBag.SupplierCount = await _db.Suppliers.CountAsync();
            ViewBag.ReceiptCount = await _db.Receipts.CountAsync();
            ViewBag.IssueCount = await _db.Issues.CountAsync();

            ViewBag.LowStockItems = await _db.Materials
                .Include(x => x.Category)
                .Where(x => x.Quantity <= x.MinStock)
                .OrderBy(x => x.Quantity)
                .Take(10)
                .ToListAsync();

            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi tải trang Dashboard");

            if (HttpContext.RequestServices
                .GetRequiredService<IWebHostEnvironment>()
                .IsDevelopment())
            {
                return Content(
                    "Lỗi tải Dashboard:\n" + ex.ToString(),
                    "text/plain");
            }

            return StatusCode(500, "Không thể tải Dashboard. Vui lòng kiểm tra log.");
        }
    }
}
