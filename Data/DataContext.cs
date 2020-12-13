using Microsoft.EntityFrameworkCore;
using JWTAuth.Models;
using System;

namespace JWTAuth.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Name)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasData(new User
                {
                    Id = 1,
                    Name = "admin@admin.com",
                    DateCreated = DateTime.Now.ToUniversalTime().ToString(),
                    ActivateKey = "",
                    PasswordHash = "AQAAAAEAACcQAAAAEEuZMQSPyBWSA+9sPwLsJvEeL3wMoqj2XFuPs8dfappQ0AXbs9cRzN9/+Cb76U+j4g==",
                    Status = UserStatus.Enabled
                });
        }
    }
}