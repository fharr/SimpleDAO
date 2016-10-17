using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SimpleDAO.Tests.DAL.EFCore.Mapping
{
    public partial class DbModel : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"DataSource=Data\Test.sqlite");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Collection>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
                entity.HasMany(e => e.Products).WithOne(e => e.Collection);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Price).HasColumnType("FLOAT");
            });
        }

        public virtual DbSet<Collection> Collection { get; set; }
        public virtual DbSet<Product> Product { get; set; }
    }
}