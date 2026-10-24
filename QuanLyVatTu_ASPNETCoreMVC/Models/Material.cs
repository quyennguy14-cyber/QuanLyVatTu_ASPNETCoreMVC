using System.ComponentModel.DataAnnotations;
namespace QuanLyVatTu.Models;
public class Material
{
    public int Id { get; set; }
    [Required, StringLength(20)]
    [Display(Name = "Mã vật tư")]
    public string Code { get; set; } = "";
    [Required, StringLength(150)]
    [Display(Name = "Tên vật tư")]
    public string Name { get; set; } = "";
    [Required, StringLength(30)]
    [Display(Name = "Đơn vị tính")]
    public string Unit { get; set; } = "";
    [Range(0, int.MaxValue)]
    [Display(Name = "Số lượng tồn")]
    public int Quantity { get; set; }
    [Range(0, 999999999)]
    [Display(Name = "Đơn giá")]
    public decimal UnitPrice { get; set; }
    [Range(0, int.MaxValue)]
    [Display(Name = "Tồn kho tối thiểu")]
    public int MinStock { get; set; } = 5;
    [Display(Name = "Danh mục")]
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    [Display(Name = "Nhà cung cấp")]
    public int SupplierId { get; set; }
    public Supplier? Supplier { get; set; }
}
