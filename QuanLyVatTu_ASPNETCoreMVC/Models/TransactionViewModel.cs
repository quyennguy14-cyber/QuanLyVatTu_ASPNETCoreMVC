using System.ComponentModel.DataAnnotations;
namespace QuanLyVatTu.Models;
public class TransactionViewModel
{
    [Required, StringLength(30)]
    [Display(Name = "Mã phiếu")]
    public string Code { get; set; } = "";
    [DataType(DataType.Date)]
    [Display(Name = "Ngày lập")]
    public DateTime Date { get; set; } = DateTime.Today;
    [StringLength(250)]
    [Display(Name = "Ghi chú")]
    public string? Note { get; set; }
    [Required, Display(Name = "Vật tư")]
    public int MaterialId { get; set; }
    [Range(1, 1000000, ErrorMessage = "Số lượng phải lớn hơn 0.")]
    [Display(Name = "Số lượng")]
    public int Quantity { get; set; }
    [Range(0, 999999999)]
    [Display(Name = "Đơn giá")]
    public decimal UnitPrice { get; set; }
}
