using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
// ReSharper disable InconsistentNaming
namespace WhoOwesWhat.DataProvider.Entity
{
    [Table("Transaction")]
    public abstract class Transaction
    {

        [Key]
        public int TransactionId { get; set; }

        public bool IsAmountSetManually { get; set; }
        public string Amount { get; set; }

        public virtual Post Post { get; set; }
        public virtual Person Person { get; set; }
    }

    public class Payer : Transaction
    {
    }    
    
    public class Consumer : Transaction
    {
    }
}

