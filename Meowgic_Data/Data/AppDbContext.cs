using Meowgic.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Data
{
    public class AppDbContext : IdentityDbContext<Account>, IAppDbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public virtual DbSet<Account> Accounts { get; set; }

        public virtual DbSet<Card> Cards { get; set; }

        public virtual DbSet<CardMeaning> CardMeanings { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<OrderDetail> OrderDetails { get; set; }

        public virtual DbSet<Promotion> Promotions { get; set; }

        public virtual DbSet<Question> Questions { get; set; }

        public virtual DbSet<TarotService> Services { get; set; }

        public DatabaseFacade DatabaseFacade => throw new NotImplementedException();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                optionsBuilder.UseSqlServer(configuration.GetConnectionString("Default"));
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Order>(entity =>
            {
                // Sử dụng HasColumnType để xác định kiểu cột SQL
                entity.Property(e => e.TotalPrice).HasColumnType("decimal(18,2)");

                // Hoặc sử dụng HasPrecision (nếu dùng EF Core 5.0+)
                // entity.Property(e => e.TotalPrice).HasPrecision(18, 2);
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                // Cấu hình các khóa ngoại với ON DELETE NO ACTION
                entity.HasOne(e => e.Order)
                    .WithMany(o => o.OrderDetails)
                    .HasForeignKey(e => e.OrderId)
                    .OnDelete(DeleteBehavior.NoAction);  // Đặt ON DELETE NO ACTION cho OrderId

                entity.HasOne(e => e.Service)
                    .WithMany(s => s.OrderDetails)
                    .HasForeignKey(e => e.ServiceId)
                    .OnDelete(DeleteBehavior.NoAction);  // Đặt ON DELETE NO ACTION cho ServiceId
            });
        }
        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new ApplicationException("Error saving changes.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unexpected error occurred.", ex);
            }
        }
        public EntityEntry Add(object entity)
        {
            return base.Add(entity);
        }
    }
}
