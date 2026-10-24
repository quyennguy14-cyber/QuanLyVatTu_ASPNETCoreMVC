using System.ComponentModel.DataAnnotations;
namespace QuanLyVatTu.Models;
public class AppUser
{
    public int Id { get; set; }
    [Required, StringLength(50)]
    [Display(Name = "Tên đăng nhập")]
    public string Username { get; set; } = "";
    [Required]
    public string PasswordHash { get; set; } = "";
    [Required, StringLength(100)]
    [Display(Name = "Họ tên")]
    public string FullName { get; set; } = "";
    [Required, StringLength(20)]
    [Display(Name = "Vai trò")]
    public string Role { get; set; } = "Staff";
}
