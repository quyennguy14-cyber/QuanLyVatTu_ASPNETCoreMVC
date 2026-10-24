using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyVatTu.Data;
namespace QuanLyVatTu.Controllers;

[ApiController]
[Route("api/v1/vattu")]
public class MaterialsApiController : ControllerBase
{
    private readonly AppDbContext _db;
    public MaterialsApiController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? q)
    {
        var query = _db.Materials.Include(x => x.Category).AsQueryable();
        if (!string.IsNullOrWhiteSpace(q)) query = query.Where(x => x.Code.Contains(q) || x.Name.Contains(q));
        var result = await query.Select(x => new {
            x.Id, x.Code, x.Name, x.Unit, x.Quantity, x.UnitPrice, Category = x.Category!.Name,
            LowStock = x.Quantity <= x.MinStock
        }).ToListAsync();
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var x = await _db.Materials.Include(m => m.Category).FirstOrDefaultAsync(m => m.Id == id);
        return x == null ? NotFound() : Ok(new { x.Id, x.Code, x.Name, x.Unit, x.Quantity, x.UnitPrice, Category = x.Category!.Name });
    }
}
