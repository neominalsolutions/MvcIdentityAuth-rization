using Microsoft.EntityFrameworkCore;
using MvcLab4.Entities;

namespace MvcLab4.Entities
{
  public class ApplicationDbContext:DbContext
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      // Fluent Api ile Configuration örneği
      // pk productId
      modelBuilder.Entity<Product>().HasKey(x => x.ProductId);

      // 100 karakter Name isminde not null
      modelBuilder.Entity<Product>().Property(x => x.ProductName)
        .HasColumnName("Name")
        .IsRequired()
        .HasMaxLength(100);

      // unique index tanımı
      modelBuilder.Entity<Product>().HasIndex(x => x.ProductName).IsUnique();

      // one to many ilişki foreing key tanımı
      modelBuilder.Entity<Product>().HasOne(x => x.Category).WithMany(x => x.Products).HasForeignKey(x => x.CategoryId);

      base.OnModelCreating(modelBuilder);
    }

  }
}
