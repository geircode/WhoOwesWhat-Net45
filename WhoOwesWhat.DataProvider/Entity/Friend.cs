using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
// ReSharper disable InconsistentNaming
namespace WhoOwesWhat.DataProvider.Entity
{
    [Table("Friend")]
    public class Friend
    {
        public Friend()
        {
        }

        [Key]
        public int FriendId { get; set; }

        public bool IsDeleted { get; set; }

        //public virtual int OwnerId { get; set; }
        //public virtual int PersonId { get; set; }

        //[ForeignKey("PersonId")]
        public virtual Person Person { get; set; }

        //[ForeignKey("OwnerId")]
        public virtual Person Owner { get; set; }


    }
}