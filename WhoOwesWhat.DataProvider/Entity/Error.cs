using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace WhoOwesWhat.DataProvider.Entity
{
    [Table("Error")]
    public class Error
    {
        public Error()
        {
        }

        [Key]
        public int ErrorId { get; set; }

        public DateTime Created { get; set; }

        public string Message { get; set; }

        public string ErrorJson { get; set; }
    }
}