using System.ComponentModel.DataAnnotations;
namespace QuanLyVatTu.Models;
public class Supplier
{
    public int Id { get; set; }
    [Required, StringLength(150)]
    [Display(Name = "Tên nhà cung cấp")]
    public string Name { get; set; } = "";
    [Phone, StringLength(20)]
    [Display(Name = "Điện thoại")]
    public string? Phone { get; set; }
    [EmailAddress, StringLength(120)]
    [Display(Name = "Email")]
    public string? Email { get; set; }
    [StringLength(250)]
    [Display(Name = "Địa chỉ")]
    public string? Address { get; set; }
    public List<Material> Materials { get; set; } = new();
}
