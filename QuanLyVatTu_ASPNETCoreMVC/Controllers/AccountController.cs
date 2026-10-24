
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyVatTu.Data;
using QuanLyVatTu.Models;

namespace QuanLyVatTu.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _db;

        public AccountController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                // Chuẩn hóa tên đăng nhập
                string username = (model.Username ?? "").Trim();
                string password = model.Password ?? "";

                // Mã hóa mật khẩu bằng SHA256
                string hash = Convert.ToHexString(
                    SHA256.HashData(Encoding.UTF8.GetBytes(password))
                );

                // Tìm tài khoản theo tên đăng nhập
                var user = await _db.AppUsers
                    .FirstOrDefaultAsync(x => x.Username == username);

                // Ghi log để kiểm tra nếu đăng nhập thất bại
                Console.WriteLine(
                    $"[LOGIN] Server={_db.Database.GetDbConnection().DataSource}"
                );
                Console.WriteLine(
                    $"[LOGIN] Database={_db.Database.GetDbConnection().Database}"
                );
                Console.WriteLine(
                    $"[LOGIN] Username=[{username}]"
                );
                Console.WriteLine(
                    $"[LOGIN] UserFound={user != null}"
                );

                if (user == null ||
                    !string.Equals(
                        user.PasswordHash?.Trim(),
                        hash,
                        StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "Tên đăng nhập hoặc mật khẩu không đúng."
                    );

                    return View(model);
                }

                // Đăng nhập thành công
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("FullName", user.FullName);
                HttpContext.Session.SetString("Role", user.Role);

                if (model.RememberMe)
                {
                    Response.Cookies.Append(
                        "RememberUser",
                        user.Username,
                        new CookieOptions
                        {
                            Expires = DateTimeOffset.Now.AddDays(7),
                            HttpOnly = true,
                            IsEssential = true,
                            SameSite = SameSiteMode.Lax
                        }
                    );
                }
                else
                {
                    Response.Cookies.Delete("RememberUser");
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // Chỉ ghi chi tiết lỗi vào log phía máy chủ
                Console.WriteLine($"[LOGIN ERROR] {ex}");

                ModelState.AddModelError(
                    string.Empty,
                    "Không thể đăng nhập. Hãy kiểm tra kết nối cơ sở dữ liệu."
                );

                return View(model);
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            Response.Cookies.Delete("RememberUser");

            return RedirectToAction("Login");
        }
    }
}
