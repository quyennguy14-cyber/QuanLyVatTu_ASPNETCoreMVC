using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyVatTu.Data;
using QuanLyVatTu.Models;
namespace QuanLyVatTu.Controllers;
public class IssuesController : Controller
{
    private readonly AppDbContext _db;
    public IssuesController(AppDbContext db) => _db = db;
    public async Task<IActionResult> Index() => View(await _db.Issues.Include(x => x.Details).OrderByDescending(x => x.Date).ToListAsync());
    public async Task<IActionResult> Create()
    {
        await FillMaterials(); return View(new TransactionViewModel { Code = "PX" + DateTime.Now.ToString("yyyyMMddHHmmss") });
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TransactionViewModel model)
    {
        if (await _db.Issues.AnyAsync(x => x.Code == model.Code)) ModelState.AddModelError("Code", "Mã phiếu đã tồn tại.");
        if (ModelState.IsValid)
        {
            var material = await _db.Materials.FindAsync(model.MaterialId);
            if (material == null) ModelState.AddModelError("MaterialId", "Vật tư không tồn tại.");
            else if (material.Quantity < model.Quantity) ModelState.AddModelError("Quantity", $"Tồn kho chỉ còn {material.Quantity} {material.Unit}.");
            else
            {
                using var tx = await _db.Database.BeginTransactionAsync();
                var issue = new Issue { Code = model.Code, Date = model.Date, Note = model.Note };
                _db.Issues.Add(issue);
                material.Quantity -= model.Quantity;
                _db.IssueDetails.Add(new IssueDetail { Issue = issue, MaterialId = model.MaterialId, Quantity = model.Quantity, UnitPrice = material.UnitPrice });
                await _db.SaveChangesAsync(); await tx.CommitAsync();
                TempData["Message"] = "Lập phiếu xuất thành công, tồn kho đã giảm.";
                return RedirectToAction(nameof(Index));
            }
        }
        await FillMaterials(model.MaterialId); return View(model);
    }
    private async Task FillMaterials(int? id = null) => ViewBag.MaterialId = new SelectList(await _db.Materials.OrderBy(x => x.Name).ToListAsync(), "Id", "Name", id);
}
