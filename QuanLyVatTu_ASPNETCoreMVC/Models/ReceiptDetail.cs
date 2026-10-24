using System.ComponentModel.DataAnnotations;
namespace QuanLyVatTu.Models;
public class ReceiptDetail
{
    public int Id { get; set; }
    public int ReceiptId { get; set; }
    public Receipt? Receipt { get; set; }
    [Display(Name = "Vật tư")]
    public int MaterialId { get; set; }
    public Material? Material { get; set; }
    [Range(1, int.MaxValue)]
    [Display(Name = "Số lượng nhập")]
    public int Quantity { get; set; }
    [Range(0, 999999999)]
    [Display(Name = "Đơn giá nhập")]
    public decimal UnitPrice { get; set; }
}
