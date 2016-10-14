namespace SimpleDAO.Tests.DAL.EF6.Mapping
{
    using AutoMapper;
    using Domain;
    using SimpleDAO.EF6.Mapping;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq.Expressions;

    [Table("Collection")]
    public partial class Collection : AutoMappableEntity<Collection, CollectionDomain>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Collection()
        {
            Products = new HashSet<Product>();
        }

        public long Id { get; set; }

        [Required]
        [StringLength(2147483647)]
        public string Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products { get; set; }

        protected override void OnConvert(CollectionDomain domain)
        {
            base.OnConvert(domain);
            domain.NbProducts = this.Products.Count;
        }
    }
}
