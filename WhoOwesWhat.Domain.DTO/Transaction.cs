using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhoOwesWhat.Domain.DTO
{
    public abstract class Transaction
    {
        public Guid PostGuid { get; set; }
        public Guid PersonGuid { get; set; }
        public string Displayname { get; set; }

        public bool IsAmountSetManually { get; set; }
        public string AmountSetManually { get; set; }

    }    
    
    public class PayerTransaction : Transaction
    {
    }    
    
    public class ConsumerTransaction : Transaction
    {
    }
}
