using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WhoOwesWhat.DataProvider.Entity;

// ReSharper disable InconsistentNaming
namespace WhoOwesWhat.DataProvider.Entity
{
    [Table("Friendrequest")]
    public class Friendrequest
    {
        public Friendrequest()
        {
        }

        [Key]
        public int FriendrequestId { get; set; }

        public virtual Person PersonRequested { get; set; }

        public virtual Person RequesterPerson { get; set; }


    }
}