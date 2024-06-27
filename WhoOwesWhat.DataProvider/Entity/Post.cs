using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
// ReSharper disable InconsistentNaming
namespace WhoOwesWhat.DataProvider.Entity
{
    [Table("Post")]
    public class Post
    {
        public Post()
        {
        }

        [Key]
        public int PostId { get; set; }

        public Guid PostGuid { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Description { get; set; }
        public string TotalCost { get; set; }
        public string Iso4217CurrencyCode { get; set; }
        public int Version { get; set; }
        public DateTime VersionUpdated { get; set; }
        public bool IsDeleted { get; set; }
        public string Comment { get; set; }

        public DateTime LastUpdated { get; set; }
        public DateTime Created { get; set; }

        public virtual Group Group { get; set; }
        public virtual Person LastUpdatedBy { get; set; }
        public virtual Person CreatedBy { get; set; }

        [InverseProperty("Post")]
        public virtual ICollection<Transaction> Transactions { get; set; }

    }


}