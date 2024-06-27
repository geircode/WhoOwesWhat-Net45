using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhoOwesWhat.DataProvider.Entity
{
    [Table("Person")]
    public class Person
    {
        public Person()
        {
        }

        [Key]
        public int PersonId { get; set; }

        [Required]
        public Guid PersonGuid { get; set; }

        [Required]
        public string Displayname { get; set; }

        public string Mobile { get; set; }
        public bool IsDeleted { get; set; }

        [InverseProperty("Person")]
        public virtual ICollection<Friend> IsAFriend { get; set; }

        [InverseProperty("Owner")]
        public virtual ICollection<Friend> HasFriend { get; set; }

        [InverseProperty("PersonRequested")]
        public virtual ICollection<Friendrequest> IsAPersonRequested { get; set; }

        [InverseProperty("RequesterPerson")]
        public virtual ICollection<Friendrequest> HasFriendrequests { get; set; }

        [InverseProperty("LastUpdatedBy")]
        public virtual ICollection<Post> PostsLastUpdatedBy { get; set; }

        [InverseProperty("CreatedBy")]
        public virtual ICollection<Post> PostsCreatedBy { get; set; }

        [InverseProperty("CreatedBy")]
        public virtual ICollection<Group> GroupsCreatedBy { get; set; }

        [InverseProperty("Person")]
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}