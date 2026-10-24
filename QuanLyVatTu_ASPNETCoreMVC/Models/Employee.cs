using System.ComponentModel.DataAnnotations;
namespace QuanLyVatTu.Models;
public class Employee
{
    public int Id { get; set; }
    [Required, StringLength(100)]
    [Display(Name = "Họ tên")]
    public string FullName { get; set; } = "";
    [Phone, StringLength(20)]
    [Display(Name = "Điện thoại")]
    public string? Phone { get; set; }
    [StringLength(100)]
    [Display(Name = "Bộ phận")]
    public string? Department { get; set; }
}
