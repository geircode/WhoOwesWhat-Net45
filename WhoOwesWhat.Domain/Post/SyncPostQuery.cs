using System;
using System.Collections.Generic;
using System.Linq;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.Domain.DTO;
using WhoOwesWhat.Domain.Interfaces;

namespace WhoOwesWhat.Domain.Post
{
    public class SyncPostQuery : ISyncPostQuery
    {
        private readonly IPostQuery _postQuery;
        private readonly IUserCredentialQuery _userCredentialQuery;
        private readonly ITransactionQuery _transactionQuery;

        public SyncPostQuery(
            IPostQuery postQuery
            , IUserCredentialQuery userCredentialQuery
            , ITransactionQuery transactionQuery
            )
        {
            _postQuery = postQuery;
            _userCredentialQuery = userCredentialQuery;
            _transactionQuery = transactionQuery;
        }


        public class SyncPostQueryException : Exception
        {
            public SyncPostQueryException(string message)
                : base(message)
            {
            }
        }

        public List<DTO.Post> GetPostConflicts(string username, List<SyncPostModel> syncPostModels)
        {
            List<DTO.Post> postConflicts = new List<DTO.Post>();
            foreach (var syncPostModel in syncPostModels)
            {
                var post = _postQuery.GetPostByPostGuid(syncPostModel.PostGuid);
                if (post != null)
                {
                    //ValidateAuthorizationUsername(username, post);
                    // TODO: Possible security flaw. Gains access to any Post given the PostGuid.
                    // Commented because a deleted Post does not have any Transactions. If 'username' is an OnlineFriend that has edited a Username Post that is deleted, this will throw exeception.

                    // Check for version conflict
                    if (syncPostModel.Version < post.Version)
                    {
                        // Check if App version is identical (except for Version), then the new Version will instead be sent to the App via UnsyncPosts
                        if (!IsEqualServerVersion(syncPostModel, post))
                        {
                            // add updating conflicting Post. The conflict will be resoved on the App.
                            postConflicts.Add(post);
                        }
                    }
                }
            }
            return postConflicts;
        }

        /// <summary>
        /// UserPerson needs to be ether Creator or part of the Transactions to be able to create or update a Post
        /// </summary>
        public void ValidateAuthorizationUsername(string username, DTO.Post model)
        {
            var userPerson = _userCredentialQuery.GetUserCredential(username);

            var isCreator = model.CreatedByPersonGuid == userPerson.Person.PersonGuid;
            var isInUseTransactions = model.PayerTransactions.Any(a => a.PersonGuid == userPerson.Person.PersonGuid);
            isInUseTransactions = isInUseTransactions || model.ConsumerTransactions.Any(a => a.PersonGuid == userPerson.Person.PersonGuid);
            if (!isCreator && !isInUseTransactions)
            {
                throw new SyncPostQueryException(String.Format("UserPerson ({0}) is not authorized to create or edit this Post: {1}", userPerson.Person.PersonGuid, model.PostGuid));
            }
        }


        public List<DTO.Post> GetUnsynchronizedPosts(string username, DateTime? lastSynchronizedToServer)
        {
            // Get all Posts created By personGuid
            // Get all Posts where personGuid is used in a Transactions 
            var user = _userCredentialQuery.GetUserCredential(username);

            var postsToApp = new List<DTO.Post>();

            List<DTO.Post> postsCreatedBy = _postQuery.GetPostsCreatedByAndUpdatedAfter(user.Person.PersonGuid, lastSynchronizedToServer);
            postsToApp.AddRange(postsCreatedBy);
            List<DTO.Post> postsInUseBy;
            if (lastSynchronizedToServer.HasValue)
            {
                postsInUseBy = _transactionQuery.GetPostsInUseByPersonGuidAndUpdatedAfter(user.Person.PersonGuid, lastSynchronizedToServer.Value);
            }
            else
            {
                postsInUseBy = _transactionQuery.GetPostsInUseByPersonGuid(user.Person.PersonGuid);
            }
            postsToApp.AddRange(postsInUseBy);
            var comparer = new PostComparer();
            postsToApp = postsToApp.Distinct(comparer).ToList();

            return postsToApp;

        }

        public List<DTO.Post> GetUnsynchronizedPostsWithoutPostConflicts(string username, DateTime? lastSynchronizedToServer, List<DTO.Post> postConflicts)
        {
            var unsynchronizedPosts = GetUnsynchronizedPosts(username, lastSynchronizedToServer);
            return GetFilterPostConflictsFromUnsynchronizedPosts(postConflicts, unsynchronizedPosts);
        }

        public List<DTO.Post> GetFilterPostConflictsFromUnsynchronizedPosts(List<DTO.Post> postConflicts, List<DTO.Post> unsynchronizedPosts)
        {
            var postConflictGroup = postConflicts.ToLookup(a => a.PostGuid);
            return unsynchronizedPosts.Where(a => !postConflictGroup.Contains(a.PostGuid)).ToList();
        }

        public class PostComparer : IEqualityComparer<DTO.Post>
        {
            public bool Equals(DTO.Post x, DTO.Post y)
            {
                return x.PostGuid == y.PostGuid;
            }

            public int GetHashCode(DTO.Post obj)
            {
                return 1;
            }
        }

        public bool IsEqualServerVersion(SyncPostModel syncPostModel, DTO.Post post)
        {
            var isNotEqual = post.PostGuid != syncPostModel.PostGuid;
            isNotEqual = isNotEqual || post.PurchaseDate != syncPostModel.PurchaseDate;
            isNotEqual = isNotEqual || post.Description != syncPostModel.Description;
            isNotEqual = isNotEqual || post.TotalCost != syncPostModel.TotalCost;
            isNotEqual = isNotEqual || post.Iso4217CurrencyCode != syncPostModel.Iso4217CurrencyCode;
            isNotEqual = isNotEqual || post.IsDeleted != syncPostModel.IsDeleted;
            isNotEqual = isNotEqual || post.Comment != syncPostModel.Comment;
            isNotEqual = isNotEqual || post.GroupGuid != syncPostModel.GroupGuid;
            isNotEqual = isNotEqual || post.LastUpdatedByPersonGuid != syncPostModel.LastUpdatedByPersonGuid;
            isNotEqual = isNotEqual || post.LastUpdated != syncPostModel.LastUpdated;
            isNotEqual = isNotEqual || post.CreatedByPersonGuid != syncPostModel.CreatedByPersonGuid;
            isNotEqual = isNotEqual || post.Created != syncPostModel.Created;

            foreach (var payerTransaction in post.PayerTransactions)
            {
                PayerTransaction transaction = payerTransaction;
                var syncPayerTransaction = syncPostModel.Payers.SingleOrDefault(a => a.PersonGuid == transaction.PersonGuid);
                if (syncPayerTransaction == null)
                {
                    isNotEqual = true;
                    break;
                }
                isNotEqual = isNotEqual || transaction.AmountSetManually != syncPayerTransaction.Amount;
                isNotEqual = isNotEqual || transaction.IsAmountSetManually != syncPayerTransaction.IsAmountSetManually;
            }            
            
            foreach (var consumerTransaction in post.ConsumerTransactions)
            {
                ConsumerTransaction transaction = consumerTransaction;
                var syncConsumerTransaction = syncPostModel.Consumers.SingleOrDefault(a => a.PersonGuid == transaction.PersonGuid);
                if (syncConsumerTransaction == null)
                {
                    isNotEqual = true;
                    break;
                }
                isNotEqual = isNotEqual || transaction.AmountSetManually != syncConsumerTransaction.Amount;
                isNotEqual = isNotEqual || transaction.IsAmountSetManually != syncConsumerTransaction.IsAmountSetManually;
            }

            return !isNotEqual;
        }
    }




}
