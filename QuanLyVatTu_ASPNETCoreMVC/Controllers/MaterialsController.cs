using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyVatTu.Data;
using QuanLyVatTu.Models;

namespace QuanLyVatTu.Controllers;
public class MaterialsController : Controller
{
    private readonly AppDbContext _db;
    public MaterialsController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index(string? q, int? categoryId, int page = 1)
    {
        const int pageSize = 8;
        var query = _db.Materials.Include(x => x.Category).Include(x => x.Supplier).AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(x => x.Code.Contains(q) || x.Name.Contains(q));
        if (categoryId.HasValue) query = query.Where(x => x.CategoryId == categoryId.Value);
        ViewBag.Categories = new SelectList(await _db.Categories.ToListAsync(), "Id", "Name", categoryId);
        ViewBag.Q = q; ViewBag.CategoryId = categoryId;
        ViewBag.Page = page; ViewBag.TotalPages = (int)Math.Ceiling(await query.CountAsync() / (double)pageSize);
        return View(await query.OrderBy(x => x.Name).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var item = await _db.Materials.Include(x => x.Category).Include(x => x.Supplier).FirstOrDefaultAsync(x => x.Id == id);
        return item == null ? NotFound() : View(item);
    }

    public async Task<IActionResult> Create()
    {
        await FillLists(); return View(new Material());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Code,Name,Unit,Quantity,UnitPrice,MinStock,CategoryId,SupplierId")] Material item)
    {
        if (await _db.Materials.AnyAsync(x => x.Code == item.Code)) ModelState.AddModelError("Code", "Mã vật tư đã tồn tại.");
        if (ModelState.IsValid) { _db.Add(item); await _db.SaveChangesAsync(); TempData["Message"] = "Đã thêm vật tư."; return RedirectToAction(nameof(Index)); }
        await FillLists(item.CategoryId, item.SupplierId); return View(item);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var item = await _db.Materials.FindAsync(id);
        if (item == null) return NotFound();
        await FillLists(item.CategoryId, item.SupplierId); return View(item);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Code,Name,Unit,Quantity,UnitPrice,MinStock,CategoryId,SupplierId")] Material item)
    {
        if (id != item.Id) return NotFound();
        if (await _db.Materials.AnyAsync(x => x.Code == item.Code && x.Id != id)) ModelState.AddModelError("Code", "Mã vật tư đã tồn tại.");
        if (ModelState.IsValid) { _db.Update(item); await _db.SaveChangesAsync(); TempData["Message"] = "Đã cập nhật vật tư."; return RedirectToAction(nameof(Index)); }
        await FillLists(item.CategoryId, item.SupplierId); return View(item);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var item = await _db.Materials.Include(x => x.Category).Include(x => x.Supplier).FirstOrDefaultAsync(x => x.Id == id);
        return item == null ? NotFound() : View(item);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await _db.Materials.FindAsync(id);
        if (item != null)
        {
            bool used = await _db.ReceiptDetails.AnyAsync(x => x.MaterialId == id) || await _db.IssueDetails.AnyAsync(x => x.MaterialId == id);
            if (used) TempData["Message"] = "Không thể xóa vật tư đã có giao dịch kho.";
            else { _db.Materials.Remove(item); await _db.SaveChangesAsync(); TempData["Message"] = "Đã xóa vật tư."; }
        }
        return RedirectToAction(nameof(Index));
    }

    private async Task FillLists(int? categoryId = null, int? supplierId = null)
    {
        ViewBag.CategoryId = new SelectList(await _db.Categories.OrderBy(x => x.Name).ToListAsync(), "Id", "Name", categoryId);
        ViewBag.SupplierId = new SelectList(await _db.Suppliers.OrderBy(x => x.Name).ToListAsync(), "Id", "Name", supplierId);
    }
}
