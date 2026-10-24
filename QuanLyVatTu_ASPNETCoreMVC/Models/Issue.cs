using System.ComponentModel.DataAnnotations;
namespace QuanLyVatTu.Models;
public class Issue
{
    public int Id { get; set; }
    [Required, StringLength(30)]
    [Display(Name = "Mã phiếu xuất")]
    public string Code { get; set; } = "";
    [Display(Name = "Ngày xuất")]
    public DateTime Date { get; set; } = DateTime.Today;
    [StringLength(250)]
    [Display(Name = "Lý do xuất")]
    public string? Note { get; set; }
    public List<IssueDetail> Details { get; set; } = new();
}
