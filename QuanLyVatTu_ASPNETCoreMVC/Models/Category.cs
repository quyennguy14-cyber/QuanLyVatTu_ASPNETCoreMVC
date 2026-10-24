using System.ComponentModel.DataAnnotations;
namespace QuanLyVatTu.Models;
public class Category
{
    public int Id { get; set; }
    [Required, StringLength(100, ErrorMessage = "Tên danh mục tối đa 100 ký tự.")]
    [Display(Name = "Tên danh mục")]
    public string Name { get; set; } = "";
    [StringLength(300)]
    [Display(Name = "Mô tả")]
    public string? Description { get; set; }
    public List<Material> Materials { get; set; } = new();
}
