namespace SimpleDAO.Tests.DAL.EF6.Mapping
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model : DbContext
    {
        public Model()
            : base(@"data source='Data\EF6.Test.sqlite'")
        {
        }

        public virtual DbSet<Collection> Collection { get; set; }
        public virtual DbSet<Product> Product { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Collection>()
                .HasMany(e => e.Products)
                .WithRequired(e => e.Collection)
                .WillCascadeOnDelete(false);
        }
    }
}
