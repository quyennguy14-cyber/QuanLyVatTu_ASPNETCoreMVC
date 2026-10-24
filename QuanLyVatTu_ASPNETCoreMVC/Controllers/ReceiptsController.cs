using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyVatTu.Data;
using QuanLyVatTu.Models;
namespace QuanLyVatTu.Controllers;
public class ReceiptsController : Controller
{
    private readonly AppDbContext _db;
    public ReceiptsController(AppDbContext db) => _db = db;
    public async Task<IActionResult> Index() => View(await _db.Receipts.Include(x => x.Details).OrderByDescending(x => x.Date).ToListAsync());
    public async Task<IActionResult> Create()
    {
        await FillMaterials(); return View(new TransactionViewModel { Code = "PN" + DateTime.Now.ToString("yyyyMMddHHmmss"), UnitPrice = 0 });
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TransactionViewModel model)
    {
        if (await _db.Receipts.AnyAsync(x => x.Code == model.Code)) ModelState.AddModelError("Code", "Mã phiếu đã tồn tại.");
        if (ModelState.IsValid)
        {
            var material = await _db.Materials.FindAsync(model.MaterialId);
            if (material == null) ModelState.AddModelError("MaterialId", "Vật tư không tồn tại.");
            else
            {
                using var tx = await _db.Database.BeginTransactionAsync();
                var receipt = new Receipt { Code = model.Code, Date = model.Date, Note = model.Note };
                _db.Receipts.Add(receipt);
                material.Quantity += model.Quantity;
                _db.ReceiptDetails.Add(new ReceiptDetail { Receipt = receipt, MaterialId = model.MaterialId, Quantity = model.Quantity, UnitPrice = model.UnitPrice });
                await _db.SaveChangesAsync(); await tx.CommitAsync();
                TempData["Message"] = "Lập phiếu nhập thành công, tồn kho đã tăng.";
                return RedirectToAction(nameof(Index));
            }
        }
        await FillMaterials(model.MaterialId); return View(model);
    }
    private async Task FillMaterials(int? id = null) => ViewBag.MaterialId = new SelectList(await _db.Materials.OrderBy(x => x.Name).ToListAsync(), "Id", "Name", id);
}
