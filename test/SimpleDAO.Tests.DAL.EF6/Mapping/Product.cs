namespace SimpleDAO.Tests.DAL.EF6.Mapping
{
    using Domain;
    using SimpleDAO.EF6.Mapping;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product : AutoMappableEntity<Product, ProductDomain>
    {
        public long Id { get; set; }

        [Required]
        [StringLength(2147483647)]
        public string Name { get; set; }

        [Column(TypeName = "real")]
        public double Price { get; set; }

        public long CollectionId { get; set; }

        public virtual Collection Collection { get; set; }

        protected override void OnConvert(ProductDomain domain)
        {
            base.OnConvert(domain);
            domain.CollectionName = this.Collection.Name;
        }
    }
}
