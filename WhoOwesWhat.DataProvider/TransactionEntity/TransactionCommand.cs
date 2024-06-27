using System;
using System.Linq;
using log4net;
using WhoOwesWhat.DataProvider.Entity;
using WhoOwesWhat.DataProvider.GroupEntity;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.DataProvider.PersonEntity;
using WhoOwesWhat.DataProvider.PostEntity;
using WhoOwesWhat.Domain.DTO;
using Group = WhoOwesWhat.DataProvider.Entity.Group;
using Transaction = WhoOwesWhat.DataProvider.Entity.Transaction;

namespace WhoOwesWhat.DataProvider.TransactionEntity
{
    public class TransactionCommand : ITransactionCommand
    {
        private readonly IWhoOwesWhatContext _whoOwesWhatContext;
        private ILog _log;
        private readonly IPersonContext _personContext;
        private readonly ITransactionContext _transactionContext;
        private readonly IPostContext _postContext;

        public TransactionCommand(IWhoOwesWhatContext whoOwesWhatContext, ILog log
            , IPersonContext personContext
            , ITransactionContext transactionContext
            , ITransactionContext transactionContext2
            , IPostContext postContext
            )
        {
            var aa = ReferenceEquals(transactionContext, transactionContext2);
            var aa2 = ReferenceEquals(transactionContext.WhoOwesWhatContext, transactionContext2.WhoOwesWhatContext);

            _whoOwesWhatContext = whoOwesWhatContext;
            _log = log;
            _personContext = personContext;
            _transactionContext = transactionContext;
            _postContext = postContext;
        }

        public void SavePayerTransaction(Domain.DTO.PayerTransaction transactionDto)
        {
            SaveTransaction(new Payer(), transactionDto);
        }

        public void SaveConsumerTransaction(Domain.DTO.ConsumerTransaction transactionDto)
        {
            SaveTransaction(new Consumer(), transactionDto);
        }
        /// <summary>
        /// All Transactions to a post is Deleted before adding new ones
        /// </summary>
        public void SaveTransaction(Entity.Transaction transactionDb, Domain.DTO.Transaction transactionDto)
        {
            var person = _personContext.GetPersonByPersonGuid(transactionDto.PersonGuid);
            var post = _postContext.GetPostByPostGuid(transactionDto.PostGuid);

            Guard.NotNull(() => person, person);
            Guard.NotNull(() => post, post);

            _transactionContext.Add(transactionDb);

            transactionDb.Post = post;
            transactionDb.Person = person;
            transactionDb.Amount = transactionDto.AmountSetManually;
            transactionDb.IsAmountSetManually = transactionDto.IsAmountSetManually;

            _whoOwesWhatContext.SaveChanges();
        }

        public void DeleteTransactions(Guid postGuid)
        {
            var transactionDbs = _transactionContext.GetTransactions().Where(a => a.Post.PostGuid == postGuid);

            foreach (var transaction in transactionDbs)
            {
                _transactionContext.Delete(transaction);
            }

            _whoOwesWhatContext.SaveChanges();
        }

        public class TransactionCommandException : Exception
        {
            public TransactionCommandException(string message)
                : base(message)
            {
            }
        }
    }
}
