using System.ComponentModel.DataAnnotations;
namespace QuanLyVatTu.Models;
public class IssueDetail
{
    public int Id { get; set; }
    public int IssueId { get; set; }
    public Issue? Issue { get; set; }
    [Display(Name = "Vật tư")]
    public int MaterialId { get; set; }
    public Material? Material { get; set; }
    [Range(1, int.MaxValue)]
    [Display(Name = "Số lượng xuất")]
    public int Quantity { get; set; }
    [Range(0, 999999999)]
    [Display(Name = "Đơn giá")]
    public decimal UnitPrice { get; set; }
}
