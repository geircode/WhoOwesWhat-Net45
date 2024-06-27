using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhoOwesWhat.Domain.DTO
{
    public class Post
    {
        public Post()
        {
            PayerTransactions = new List<PayerTransaction>();
            ConsumerTransactions = new List<ConsumerTransaction>();
        }

        public Guid PostGuid { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Description { get; set; }
        public string TotalCost { get; set; }
        public string Iso4217CurrencyCode { get; set; }
        public int Version { get; set; }
        public DateTime VersionUpdated { get; set; }
        public bool IsDeleted { get; set; }
        public string Comment { get; set; }
        public Guid? GroupGuid { get; set; }
        public Guid LastUpdatedByPersonGuid { get; set; }
        public DateTime LastUpdated { get; set; }
        public Guid CreatedByPersonGuid { get; set; }
        public DateTime Created { get; set; }

        public List<PayerTransaction> PayerTransactions { get; set; }
        public List<ConsumerTransaction> ConsumerTransactions { get; set; }
    }


    public class SyncPostModel // Dette er en Parameterklasse. Skal kun brukes for å sende data _til_ en funksjon. Andre funksjoner skal ikke plutselig returnere denne klassen.
    {
        public SyncPostModel()
        {
            Payers = new List<SyncPayerModel>();
            Consumers = new List<SyncConsumerModel>();
        }

        public Guid PostGuid { get; set; } 
        public DateTime PurchaseDate { get; set; }
        public string Description { get; set; }
        public string TotalCost { get; set; }
        public string Iso4217CurrencyCode { get; set; }
        public int Version { get; set; } 
        public bool IsDeleted { get; set; } 
        public string Comment { get; set; } 
        public Guid? GroupGuid { get; set; } 
        public Guid LastUpdatedByPersonGuid { get; set; }
        public DateTime LastUpdated { get; set; } 
        public Guid CreatedByPersonGuid { get; set; }
        public DateTime Created { get; set; }

        public List<SyncPayerModel> Payers { get; set; }
        public List<SyncConsumerModel> Consumers { get; set; }

        public abstract class SyncTransactionModel
        {
            public Guid PersonGuid { get; set; }
            public bool IsAmountSetManually { get; set; }
            public string Amount { get; set; }
        }

        public class SyncPayerModel : SyncTransactionModel
        {
        }

        public class SyncConsumerModel : SyncTransactionModel
        {
        }
    }



}
