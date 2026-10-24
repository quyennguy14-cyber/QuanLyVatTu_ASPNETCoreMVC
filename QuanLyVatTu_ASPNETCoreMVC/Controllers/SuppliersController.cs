using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyVatTu.Data;
using QuanLyVatTu.Models;
namespace QuanLyVatTu.Controllers;
public class SuppliersController : Controller
{
    private readonly AppDbContext _db;
    public SuppliersController(AppDbContext db) => _db = db;
    public async Task<IActionResult> Index(string? q) => View(await _db.Suppliers.Where(x => string.IsNullOrEmpty(q) || x.Name.Contains(q)).OrderBy(x => x.Name).ToListAsync());
    public IActionResult Create() => View(new Supplier());
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Supplier model)
    {
        if (ModelState.IsValid) { _db.Add(model); await _db.SaveChangesAsync(); return RedirectToAction(nameof(Index)); }
        return View(model);
    }
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var x = await _db.Suppliers.FindAsync(id); return x == null ? NotFound() : View(x);
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Supplier model)
    {
        if (id != model.Id) return NotFound();
        if (ModelState.IsValid) { _db.Update(model); await _db.SaveChangesAsync(); return RedirectToAction(nameof(Index)); }
        return View(model);
    }
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var x = await _db.Suppliers.Include(s => s.Materials).FirstOrDefaultAsync(s => s.Id == id);
        return x == null ? NotFound() : View(x);
    }
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var x = await _db.Suppliers.Include(s => s.Materials).FirstOrDefaultAsync(s => s.Id == id);
        if (x != null && x.Materials.Count == 0) { _db.Remove(x); await _db.SaveChangesAsync(); }
        else TempData["Message"] = "Không thể xóa nhà cung cấp đang liên kết vật tư.";
        return RedirectToAction(nameof(Index));
    }
}
