using System;
using System.Linq;
using WhoOwesWhat.DataProvider.Entity;

namespace WhoOwesWhat.DataProvider.TransactionEntity
{


    public interface ITransactionContext : IContextBase
    {
        IQueryable<Transaction> GetTransactionsByPostId(int postId);
        IQueryable<Transaction> GetTransactions();
        void Add(Transaction transactionDb);
        void SaveChanges();
        void Delete(Transaction transaction);
    }

    public class TransactionContext : ITransactionContext
    {
        private readonly IWhoOwesWhatContext _whoOwesWhatContext;

        public TransactionContext(IWhoOwesWhatContext whoOwesWhatContext)
        {
            _whoOwesWhatContext = whoOwesWhatContext;
        }

        public IQueryable<Transaction> GetTransactionsByPostId(int postId)
        {
            return _whoOwesWhatContext.GetTransactionSqlRepository().GetAll().Where(a => a.Post.PostId == postId);
        }
        public IQueryable<Transaction> GetTransactions()
        {
            return _whoOwesWhatContext.GetTransactionSqlRepository().GetAll();
        }


        public void Add(Transaction transactionDb)
        {
            _whoOwesWhatContext.GetTransactionSqlRepository().Add(transactionDb);
        }

        public void SaveChanges()
        {
            _whoOwesWhatContext.SaveChanges();
        }

        public void Delete(Transaction transaction)
        {
            _whoOwesWhatContext.GetTransactionSqlRepository().Remove(transaction);
        }

        public IWhoOwesWhatContext WhoOwesWhatContext
        {
            get { return _whoOwesWhatContext; }
        }
    }
}
