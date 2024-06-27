using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
// ReSharper disable InconsistentNaming
namespace WhoOwesWhat.DataProvider.Entity
{
    [Table("Group")]
    public class Group
    {
        public Group()
        {
        }

        [Key]
        public int GroupId { get; set; }
        public Guid GroupGuid { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime VersionUpdated { get; set; }


        [InverseProperty("Group")]
        public virtual ICollection<Post> Posts { get; set; }

        public virtual Person CreatedBy { get; set; }
    }
}