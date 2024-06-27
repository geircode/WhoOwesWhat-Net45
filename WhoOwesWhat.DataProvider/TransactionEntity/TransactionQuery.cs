using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using log4net;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.DataProvider.PostEntity;
using WhoOwesWhat.Domain.DTO;
using Post = WhoOwesWhat.DataProvider.Entity.Post;

namespace WhoOwesWhat.DataProvider.TransactionEntity
{
    public class TransactionQuery : ITransactionQuery
    {
        private readonly ITransactionContext _transactionContext;
        private readonly IPostDataProviderLogic _postDataProviderLogic;

        public TransactionQuery(
            ITransactionContext transactionContext
            , IPostDataProviderLogic postDataProviderLogic
            )
        {
            _transactionContext = transactionContext;
            _postDataProviderLogic = postDataProviderLogic;
        }

        public bool IsPersonUsedInAnyPosts(Guid personGuid)
        {
            return _transactionContext.GetTransactions().Any(a => a.Person.PersonGuid == personGuid);
        }

        public List<Domain.DTO.Post> GetPostsInUseByPersonGuid(Guid personGuid)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            // TODO: sjekk om Distinct faktisk fungerer ved å legge til PersonGuid som Payer og Consumer
            var postsInUse = _transactionContext.GetTransactions().Where(b =>   b.Post.IsDeleted == false && b.Person.PersonGuid == personGuid).Select(b => b.Post).Distinct();
            foreach (var post in postsInUse)
            {
                _transactionContext.WhoOwesWhatContext.LoadProperty(post, a => a.Group);
                _transactionContext.WhoOwesWhatContext.LoadProperty(post, a => a.CreatedBy);
                _transactionContext.WhoOwesWhatContext.LoadProperty(post, a => a.LastUpdatedBy);
                _transactionContext.WhoOwesWhatContext.LoadCollection(post, a => a.Transactions);
                foreach (var transaction in post.Transactions)
                {
                    _transactionContext.WhoOwesWhatContext.LoadProperty(transaction, a => a.Person);
                }
            }

            stopwatch.Stop();
            Console.WriteLine("GetPostsInUseByPersonGuid: " + stopwatch.ElapsedMilliseconds + " ms");

            var postsDomain = new List<Domain.DTO.Post>();
            foreach (var post in postsInUse)
            {
                postsDomain.Add(_postDataProviderLogic.MapToDomain(post));
            }
            return postsDomain;
        }

        public List<Domain.DTO.Post> GetPostsInUseByPersonGuidAndUpdatedAfter(Guid personGuid, DateTime lastSynchronizedToServer)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            var postsInUse = _transactionContext.GetTransactions().Where(
                b => b.Post.VersionUpdated > lastSynchronizedToServer
                    && b.Person.PersonGuid == personGuid 
                    //&& b.Post.IsDeleted == false
                    )
                    .Select(b => b.Post).Distinct();
            foreach (var post in postsInUse)
            {
                _transactionContext.WhoOwesWhatContext.LoadProperty(post, a => a.Group);
                _transactionContext.WhoOwesWhatContext.LoadProperty(post, a => a.CreatedBy);
                _transactionContext.WhoOwesWhatContext.LoadProperty(post, a => a.LastUpdatedBy);
                _transactionContext.WhoOwesWhatContext.LoadCollection(post, a => a.Transactions);
                foreach (var transaction in post.Transactions)
                {
                    _transactionContext.WhoOwesWhatContext.LoadProperty(transaction, a => a.Person);
                }
            }

            stopwatch.Stop();
            Console.WriteLine("GetPostsInUseByPersonGuid: " + stopwatch.ElapsedMilliseconds + " ms");

            var postsDomain = new List<Domain.DTO.Post>();
            foreach (var post in postsInUse)
            {
                postsDomain.Add(_postDataProviderLogic.MapToDomain(post));
            }
            return postsDomain;
        }
    }

}
