using Microsoft.EntityFrameworkCore;
using QuanLyVatTu.Models;

namespace QuanLyVatTu.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Material> Materials => Set<Material>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<AppUser> AppUsers => Set<AppUser>();
    public DbSet<Receipt> Receipts => Set<Receipt>();
    public DbSet<ReceiptDetail> ReceiptDetails => Set<ReceiptDetail>();
    public DbSet<Issue> Issues => Set<Issue>();
    public DbSet<IssueDetail> IssueDetails => Set<IssueDetail>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);
        b.Entity<Category>().HasIndex(x => x.Name).IsUnique();
        b.Entity<Material>().HasIndex(x => x.Code).IsUnique();
        b.Entity<AppUser>().HasIndex(x => x.Username).IsUnique();
        b.Entity<ReceiptDetail>().HasOne(x => x.Receipt).WithMany(x => x.Details)
            .HasForeignKey(x => x.ReceiptId).OnDelete(DeleteBehavior.Cascade);
        b.Entity<IssueDetail>().HasOne(x => x.Issue).WithMany(x => x.Details)
            .HasForeignKey(x => x.IssueId).OnDelete(DeleteBehavior.Cascade);
        b.Entity<Material>().Property(x => x.UnitPrice).HasColumnType("decimal(18,2)");
        b.Entity<ReceiptDetail>().Property(x => x.UnitPrice).HasColumnType("decimal(18,2)");
        b.Entity<IssueDetail>().Property(x => x.UnitPrice).HasColumnType("decimal(18,2)");
    }
}
