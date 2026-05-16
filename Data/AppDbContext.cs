using ComputerManagementApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ComputerManagementApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Pc> Pcs { get; set; }
    public DbSet<Component> Components { get; set; }
    public DbSet<PcComponent> PcComponents { get; set; }
    public DbSet<ComponentType> ComponentTypes { get; set; }
    public DbSet<ComponentManufacturer> ComponentManufacturers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pc>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Name).IsRequired().HasMaxLength(50);
            e.Property(p => p.Weight).HasColumnType("float");
            e.Property(p => p.Warranty).IsRequired();
            e.Property(p => p.CreatedAt).HasColumnType("datetime").IsRequired();
            e.Property(p => p.Stock).IsRequired();
        });

        modelBuilder.Entity<ComponentType>(e =>
        {
            e.HasKey(ct => ct.Id);
            e.Property(ct => ct.Abbreviation).IsRequired().HasMaxLength(30);
            e.Property(ct => ct.Name).IsRequired().HasMaxLength(150);
        });

        modelBuilder.Entity<ComponentManufacturer>(e =>
        {
            e.HasKey(cm => cm.Id);
            e.Property(cm => cm.Abbreviation).IsRequired().HasMaxLength(30);
            e.Property(cm => cm.FullName).IsRequired().HasMaxLength(300);
            e.Property(cm => cm.FoundationDate).HasColumnType("date").IsRequired();
        });

        modelBuilder.Entity<Component>(e =>
        {
            e.HasKey(c => c.Code);
            e.Property(c => c.Code).HasColumnType("char(10)").IsRequired();
            e.Property(c => c.Name).IsRequired().HasMaxLength(300);
            e.Property(c => c.Description).HasColumnType("nvarchar(max)");

            e.HasOne(c => c.Manufacturer)
             .WithMany(m => m.Components)
             .HasForeignKey(c => c.ComponentManufacturerId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(c => c.Type)
             .WithMany(t => t.Components)
             .HasForeignKey(c => c.ComponentTypeId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<PcComponent>(e =>
        {
            e.HasKey(pc => new { pc.PcId, pc.ComponentCode });
            e.Property(pc => pc.ComponentCode).HasColumnType("char(10)");
            e.Property(pc => pc.Amount).IsRequired();

            e.HasOne(pc => pc.Pc)
             .WithMany(p => p.PcComponents)
             .HasForeignKey(pc => pc.PcId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(pc => pc.Component)
             .WithMany(c => c.PcComponents)
             .HasForeignKey(pc => pc.ComponentCode)
             .OnDelete(DeleteBehavior.Restrict);
        });

        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ComponentType>().HasData(
            new ComponentType { Id = 1, Abbreviation = "CPU", Name = "Processor" },
            new ComponentType { Id = 2, Abbreviation = "GPU", Name = "Graphics Card" },
            new ComponentType { Id = 3, Abbreviation = "RAM", Name = "Memory" },
            new ComponentType { Id = 4, Abbreviation = "SSD", Name = "Solid State Drive" }
        );

        modelBuilder.Entity<ComponentManufacturer>().HasData(
            new ComponentManufacturer { Id = 1, Abbreviation = "AMD",  FullName = "Advanced Micro Devices",  FoundationDate = new DateOnly(1969, 5, 1) },
            new ComponentManufacturer { Id = 2, Abbreviation = "NV",   FullName = "NVIDIA Corporation",      FoundationDate = new DateOnly(1993, 4, 5) },
            new ComponentManufacturer { Id = 3, Abbreviation = "COR",  FullName = "Corsair Gaming Inc.",     FoundationDate = new DateOnly(1994, 1, 1) },
            new ComponentManufacturer { Id = 4, Abbreviation = "SAM",  FullName = "Samsung Electronics",    FoundationDate = new DateOnly(1969, 1, 13) }
        );

        modelBuilder.Entity<Component>().HasData(
            new Component { Code = "CPU0000001", Name = "Ryzen 7 7800X3D",          Description = "8-core gaming processor",           ComponentManufacturerId = 1, ComponentTypeId = 1 },
            new Component { Code = "CPU0000002", Name = "Intel Core i9-14900K",     Description = "24-core high-performance processor", ComponentManufacturerId = 1, ComponentTypeId = 1 },
            new Component { Code = "GPU0000001", Name = "RTX 4080 Super",           Description = "High-end gaming graphics card",      ComponentManufacturerId = 2, ComponentTypeId = 2 },
            new Component { Code = "GPU0000002", Name = "RTX 4060",                 Description = "Mid-range gaming graphics card",     ComponentManufacturerId = 2, ComponentTypeId = 2 },
            new Component { Code = "RAM0000001", Name = "Corsair Vengeance DDR5 16GB", Description = "DDR5 RAM module 16GB",           ComponentManufacturerId = 3, ComponentTypeId = 3 },
            new Component { Code = "RAM0000002", Name = "Corsair Vengeance DDR5 32GB", Description = "DDR5 RAM module 32GB",           ComponentManufacturerId = 3, ComponentTypeId = 3 },
            new Component { Code = "SSD0000001", Name = "Samsung 990 Pro 1TB",      Description = "NVMe SSD 1TB",                      ComponentManufacturerId = 4, ComponentTypeId = 4 }
        );

        modelBuilder.Entity<Pc>().HasData(
            new Pc { Id = 1, Name = "Gaming Beast X",  Weight = 12.5, Warranty = 36, CreatedAt = new DateTime(2026, 5, 8, 9, 0, 0),   Stock = 5 },
            new Pc { Id = 2, Name = "Office Mini Pro", Weight = 4.2,  Warranty = 24, CreatedAt = new DateTime(2026, 4, 15, 13, 30, 0), Stock = 12 },
            new Pc { Id = 3, Name = "Workstation Pro", Weight = 18.0, Warranty = 48, CreatedAt = new DateTime(2026, 3, 20, 10, 0, 0),  Stock = 3 }
        );

        modelBuilder.Entity<PcComponent>().HasData(
            new PcComponent { PcId = 1, ComponentCode = "CPU0000001", Amount = 1 },
            new PcComponent { PcId = 1, ComponentCode = "GPU0000001", Amount = 1 },
            new PcComponent { PcId = 1, ComponentCode = "RAM0000001", Amount = 2 },
            new PcComponent { PcId = 1, ComponentCode = "SSD0000001", Amount = 1 },
            new PcComponent { PcId = 2, ComponentCode = "CPU0000002", Amount = 1 },
            new PcComponent { PcId = 2, ComponentCode = "GPU0000002", Amount = 1 },
            new PcComponent { PcId = 2, ComponentCode = "RAM0000002", Amount = 1 },
            new PcComponent { PcId = 3, ComponentCode = "CPU0000002", Amount = 2 },
            new PcComponent { PcId = 3, ComponentCode = "RAM0000002", Amount = 4 },
            new PcComponent { PcId = 3, ComponentCode = "SSD0000001", Amount = 2 }
        );
    }
}
