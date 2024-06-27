using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhoOwesWhat.DataProvider.Entity
{
    [Table("UserCredential")]
    public class UserCredential
    {
        public UserCredential()
        {
        }

        [Key, ForeignKey("Person")]
        public int PersonId { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public virtual Person Person { get; set; }


    }
}