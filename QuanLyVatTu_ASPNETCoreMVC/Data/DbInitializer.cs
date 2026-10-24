using System.Security.Cryptography;
using System.Text;
using QuanLyVatTu.Models;

namespace QuanLyVatTu.Data;

public static class DbInitializer
{
    public static void Seed(AppDbContext db)
    {
        if (!db.AppUsers.Any())
        {
            db.AppUsers.Add(new AppUser
            {
                Username = "admin",
                PasswordHash = Hash("Admin@123"),
                FullName = "Quản trị viên",
                Role = "Admin"
            });
        }
        if (!db.Categories.Any())
        {
            db.Categories.AddRange(
                new Category { Name = "Vật tư điện", Description = "Dây điện, công tắc, thiết bị điện" },
                new Category { Name = "Vật tư xây dựng", Description = "Xi măng, cát, thép" },
                new Category { Name = "Văn phòng phẩm", Description = "Giấy, bút và vật dụng văn phòng" }
            );
        }
        if (!db.Suppliers.Any())
        {
            db.Suppliers.Add(new Supplier { Name = "Công ty Vật tư Minh Phát", Phone = "0900000001", Address = "Hà Nội", Email = "minhphat@example.com" });
        }
        db.SaveChanges();
        if (!db.Materials.Any())
        {
            var category = db.Categories.First();
            var supplier = db.Suppliers.First();
            db.Materials.AddRange(
                new Material { Code = "VT001", Name = "Dây điện 2.5mm", Unit = "Cuộn", Quantity = 20, UnitPrice = 150000, MinStock = 5, CategoryId = category.Id, SupplierId = supplier.Id },
                new Material { Code = "VT002", Name = "Công tắc điện", Unit = "Cái", Quantity = 50, UnitPrice = 25000, MinStock = 10, CategoryId = category.Id, SupplierId = supplier.Id }
            );
            db.SaveChanges();
        }
    }

    public static string Hash(string password)
    {
        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(password)));
    }
}
