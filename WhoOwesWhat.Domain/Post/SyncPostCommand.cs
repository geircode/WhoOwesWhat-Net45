using System;
using System.Collections.Generic;
using System.Linq;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.Domain.DTO;
using WhoOwesWhat.Domain.Interfaces;

namespace WhoOwesWhat.Domain.Post
{
    public class SyncPostCommand : ISyncPostCommand
    {
        private readonly IPostQuery _postQuery;
        private readonly IPostCommand _postCommand;
        private readonly IUserCredentialQuery _userCredentialQuery;
        private readonly ITransactionCommand _transactionCommand;

        public SyncPostCommand(
            IPostQuery postQuery
            , IPostCommand postCommand
            , IUserCredentialQuery userCredentialQuery
            , ITransactionCommand transactionCommand
            )
        {
            _postQuery = postQuery;
            _postCommand = postCommand;
            _userCredentialQuery = userCredentialQuery;
            _transactionCommand = transactionCommand;
        }

        public void SyncPost(string username, SyncPostModel syncPostModel)
        {
            // Create or update Post
            // On Update: Check if username has access to Post
            // Increase Version number
            // Check for conflicts between Versions. MMF: Skip if the Post is conflicting. The conflict will be resovled on the App.
            // når f.eks. App har en dirty Post som har en version som er lavere enn server version?

            Guard.NotNullOrEmpty(() => username, username);
            Guard.NotNull(() => syncPostModel, syncPostModel);
            ValidateSyncPostModel(syncPostModel);

            var post = _postQuery.GetPostByPostGuid(syncPostModel.PostGuid);
            if (post == null)
            {
                if (syncPostModel.Version != 0)
                {
                    throw new SyncPostCommandException("New post version was not 0 but: " + syncPostModel.Version);
                }

                post = new DTO.Post();
                post.Version = 1;
            }
            else
            {
                // Check for version conflict
                if (syncPostModel.Version < post.Version)
                {
                    // skip updating conflicting Post. The conflict will be resolved on the App.
                    return;
                }
                if (syncPostModel.Version > post.Version)
                {
                    throw new SyncPostCommandException("App version of the Post is higher than the Post on the Server");
                }

                // increase Version foreach Sync
                post.Version = syncPostModel.Version + 1;
                var userPerson = _userCredentialQuery.GetUserCredential(username);
                if (syncPostModel.IsDeleted && (post.CreatedByPersonGuid != userPerson.Person.PersonGuid))
                {
                    throw new SyncPostCommandException("Only the creator of the Post can Delete the Post");
                }
                if (post.IsDeleted && syncPostModel.IsDeleted == false && (post.CreatedByPersonGuid != userPerson.Person.PersonGuid))
                {
                    // Occurs when OnlineFriend tries to modify a deleted Post on the App during PostConflict
                    throw new SyncPostCommandException("Only the creator of the Post can Undelete the Post");
                }
                ValidateAuthorizationUsername(userPerson.Person.PersonGuid, post);
            }
            post.VersionUpdated = DateTime.Now;

            post.PostGuid = syncPostModel.PostGuid;
            post.PurchaseDate = syncPostModel.PurchaseDate;
            post.Description = syncPostModel.Description;
            post.TotalCost = syncPostModel.TotalCost;
            post.Iso4217CurrencyCode = syncPostModel.Iso4217CurrencyCode;
            post.IsDeleted = syncPostModel.IsDeleted;
            post.Comment = syncPostModel.Comment;
            post.GroupGuid = syncPostModel.GroupGuid;
            post.LastUpdatedByPersonGuid = syncPostModel.LastUpdatedByPersonGuid;
            post.LastUpdated = syncPostModel.LastUpdated;
            post.CreatedByPersonGuid = syncPostModel.CreatedByPersonGuid;
            post.Created = syncPostModel.Created;

            _postCommand.SavePost(post);
            if (!post.IsDeleted)
            {
                _transactionCommand.DeleteTransactions(post.PostGuid);
                foreach (var syncPayerModel in syncPostModel.Payers)
                {
                    _transactionCommand.SavePayerTransaction(new PayerTransaction()
                    {
                        PostGuid = post.PostGuid,
                        PersonGuid = syncPayerModel.PersonGuid,
                        AmountSetManually = syncPayerModel.Amount,
                        IsAmountSetManually = syncPayerModel.IsAmountSetManually
                    });
                }

                foreach (var syncConsumerModel in syncPostModel.Consumers)
                {
                    _transactionCommand.SaveConsumerTransaction(new ConsumerTransaction()
                    {
                        PostGuid = post.PostGuid,
                        PersonGuid = syncConsumerModel.PersonGuid,
                        AmountSetManually = syncConsumerModel.Amount,
                        IsAmountSetManually = syncConsumerModel.IsAmountSetManually
                    });
                }
            }
        }


        /// <summary>
        /// UserPerson needs to be either Creator or part of the Transactions to be able to create or update a Post
        /// </summary>
        public void ValidateAuthorizationUsername(Guid personGuid, DTO.Post model)
        {
            var isCreator = model.CreatedByPersonGuid == personGuid;
            var isInUseTransactions = model.PayerTransactions.Any(a => a.PersonGuid == personGuid);
            isInUseTransactions = isInUseTransactions || model.ConsumerTransactions.Any(a => a.PersonGuid == personGuid);
            if (!isCreator && !isInUseTransactions)
            {
                throw new SyncPostCommandException(String.Format("UserPerson ({0}) is not authorized to create or edit this Post: {1}", personGuid, model.PostGuid));
            }
        }



        private void ValidateSyncPostModel(SyncPostModel model)
        {
            Guard.NotNull(() => model.PostGuid, model.PostGuid);
            Guard.NotNull(() => model.PurchaseDate, model.PurchaseDate);
            Guard.NotNullOrEmpty(() => model.Description, model.Description);
            Guard.NotNullOrEmpty(() => model.TotalCost, model.TotalCost);
            Guard.NotNullOrEmpty(() => model.Iso4217CurrencyCode, model.Iso4217CurrencyCode);
            Guard.NotNull(() => model.Version, model.Version);
            Guard.NotNull(() => model.IsDeleted, model.IsDeleted);
            Guard.NotNull(() => model.LastUpdatedByPersonGuid, model.LastUpdatedByPersonGuid);
            Guard.NotNull(() => model.LastUpdated, model.LastUpdated);

            Guard.NotNull(() => model.CreatedByPersonGuid, model.CreatedByPersonGuid);
            Guard.NotNull(() => model.Created, model.Created);
            Guard.NotNull(() => model.Payers, model.Payers);
            Guard.NotNull(() => model.Consumers, model.Consumers);
            Guard.IsValid(() => model.PostGuid, model.PostGuid, ValidatePersonGuid, "PostGuid can not be empty");

            // Transactions blir slettet på klienten når en Post blir slettet (isDeleted=true)
            if (model.IsDeleted == false)
            {
                Guard.IsValid(() => model.Payers, model.Payers, ValidateCollection, "No Payers are registered");
                Guard.IsValid(() => model.Consumers, model.Consumers, ValidateCollection, "No Consumers are registered");
            }

            Guard.IsValid(() => model.LastUpdated, model.LastUpdated, ValidateDateTime, "Not a valid time");
            Guard.IsValid(() => model.Created, model.Created, ValidateDateTime, "Not a valid time");

        }

        private bool ValidateDateTime(DateTime dateTime)
        {
            return (dateTime > new DateTime(1970, 01, 01));
        }

        private bool ValidateCollection<T>(List<T> theList)
        {
            return theList.Count > 0;
        }

        private bool ValidatePersonGuid(Guid guid)
        {
            return guid != Guid.Empty;
        }

        private bool ValidateIsTrue(bool istrue)
        {
            return istrue == true;
        }

        public class SyncPostCommandException : Exception
        {
            public SyncPostCommandException(string message)
                : base(message)
            {
            }
        }
    }
}
