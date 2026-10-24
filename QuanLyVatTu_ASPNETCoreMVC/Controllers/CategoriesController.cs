using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyVatTu.Data;
using QuanLyVatTu.Models;
namespace QuanLyVatTu.Controllers;
public class CategoriesController : Controller
{
    private readonly AppDbContext _db;
    public CategoriesController(AppDbContext db) => _db = db;
    public async Task<IActionResult> Index() => View(await _db.Categories.OrderBy(x => x.Name).ToListAsync());
    public IActionResult Create() => View(new Category());
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Category model)
    {
        if (await _db.Categories.AnyAsync(x => x.Name == model.Name)) ModelState.AddModelError("Name", "Tên danh mục đã tồn tại.");
        if (ModelState.IsValid) { _db.Add(model); await _db.SaveChangesAsync(); return RedirectToAction(nameof(Index)); }
        return View(model);
    }
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var x = await _db.Categories.FindAsync(id); return x == null ? NotFound() : View(x);
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Category model)
    {
        if (id != model.Id) return NotFound();
        if (await _db.Categories.AnyAsync(x => x.Name == model.Name && x.Id != id)) ModelState.AddModelError("Name", "Tên danh mục đã tồn tại.");
        if (ModelState.IsValid) { _db.Update(model); await _db.SaveChangesAsync(); return RedirectToAction(nameof(Index)); }
        return View(model);
    }
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var x = await _db.Categories.Include(c => c.Materials).FirstOrDefaultAsync(c => c.Id == id);
        return x == null ? NotFound() : View(x);
    }
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var x = await _db.Categories.Include(c => c.Materials).FirstOrDefaultAsync(c => c.Id == id);
        if (x != null && x.Materials.Count == 0) { _db.Remove(x); await _db.SaveChangesAsync(); }
        else TempData["Message"] = "Không thể xóa danh mục đang có vật tư.";
        return RedirectToAction(nameof(Index));
    }
}
