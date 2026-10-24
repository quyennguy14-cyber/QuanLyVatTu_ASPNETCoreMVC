using System.ComponentModel.DataAnnotations;
namespace QuanLyVatTu.Models;
public class Receipt
{
    public int Id { get; set; }
    [Required, StringLength(30)]
    [Display(Name = "Mã phiếu nhập")]
    public string Code { get; set; } = "";
    [Display(Name = "Ngày nhập")]
    public DateTime Date { get; set; } = DateTime.Today;
    [StringLength(250)]
    [Display(Name = "Ghi chú")]
    public string? Note { get; set; }
    public List<ReceiptDetail> Details { get; set; } = new();
}
