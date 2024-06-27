using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.Domain.DTO;
using WhoOwesWhat.Domain.Interfaces;
using WhoOwesWhat.Service.DTO;

namespace WhoOwesWhat.Service.Controller
{
    public interface ISyncControllerLogic
    {
        List<SyncPostModel> MapSyncPostsRequestToSyncPostModel(List<SyncPostsRequest.Post> posts);
        List<SyncPostsResponse.Post> MapPostToSyncPostsResponsePost(List<Post> postConflicts);
        List<SyncGroupModel> MapSyncGroupsRequestToSyncGroupModel(List<SyncGroupsRequest.Group> groups);
        List<SyncGroupsResponse.Group> MapGroupToSyncGroupsResponseGroup(List<Group> groups);
    }

    public class SyncControllerLogic : ISyncControllerLogic
    {
        public SyncControllerLogic(
            )
        {
        }

        public List<SyncPostModel> MapSyncPostsRequestToSyncPostModel(List<SyncPostsRequest.Post> posts)
        {
            List<SyncPostModel> syncPostModels = new List<SyncPostModel>();

            foreach (var post in posts)
            {
                var syncPostModel = new SyncPostModel();
                syncPostModel.PostGuid = post.postGuid;
                syncPostModel.PurchaseDate = post.purchaseDate;
                syncPostModel.Description = post.description;
                syncPostModel.TotalCost = post.totalCost;
                syncPostModel.Iso4217CurrencyCode = post.iso4217CurrencyCode;
                syncPostModel.Version = post.version;
                syncPostModel.IsDeleted = post.isDeleted;
                syncPostModel.Comment = post.comment;
                syncPostModel.GroupGuid = post.groupGuid;
                syncPostModel.LastUpdatedByPersonGuid = post.lastUpdatedByPersonGuid;
                syncPostModel.LastUpdated = post.lastUpdated;
                syncPostModel.CreatedByPersonGuid = post.createdByPersonGuid;
                syncPostModel.Created = post.created;

                foreach (var payerTransaction in post.payerTransactions)
                {
                    syncPostModel.Payers.Add(new SyncPostModel.SyncPayerModel()
                    {
                        PersonGuid = payerTransaction.personGuid,
                        IsAmountSetManually = payerTransaction.isAmountSetManually,
                        Amount = payerTransaction.amountSetManually,
                    });
                }

                foreach (var consumerTransaction in post.consumerTransactions)
                {
                    syncPostModel.Consumers.Add(new SyncPostModel.SyncConsumerModel()
                    {
                        PersonGuid = consumerTransaction.personGuid,
                        IsAmountSetManually = consumerTransaction.isAmountSetManually,
                        Amount = consumerTransaction.amountSetManually,
                    });
                }

                syncPostModels.Add(syncPostModel);
            }
            return syncPostModels;
        }

        //public Guid postGuid { get; set; }
        //public DateTime purchaseDate { get; set; }
        //public string description { get; set; }
        //public string totalCost { get; set; }
        //public string iso4217CurrencyCode { get; set; }
        //public int version { get; set; }
        //public bool isDeleted { get; set; }
        //public string comment { get; set; }
        //public Guid? groupGuid { get; set; }
        //public Guid lastUpdatedByPersonGuid { get; set; }
        //public DateTime lastUpdated { get; set; }
        //public Guid createdByPersonGuid { get; set; }
        //public DateTime created { get; set; }
        //public List<Transaction> payerTransactions { get; set; }
        //public List<Transaction> consumerTransactions { get; set; }

        public List<SyncPostsResponse.Post> MapPostToSyncPostsResponsePost(List<Post> posts)
        {
            List<SyncPostsResponse.Post> requestPosts = new List<SyncPostsResponse.Post>();

            foreach (var post in posts)
            {
                SyncPostsResponse.Post requestPost = new SyncPostsResponse.Post();
                requestPost.postGuid = post.PostGuid;
                requestPost.purchaseDate = post.PurchaseDate;
                requestPost.description = post.Description;
                requestPost.totalCost = post.TotalCost;
                requestPost.iso4217CurrencyCode = post.Iso4217CurrencyCode;
                requestPost.version = post.Version;
                requestPost.isDeleted = post.IsDeleted;
                requestPost.comment = post.Comment;
                requestPost.groupGuid = post.GroupGuid;
                requestPost.lastUpdatedByPersonGuid = post.LastUpdatedByPersonGuid;
                requestPost.lastUpdated = post.LastUpdated;
                requestPost.createdByPersonGuid = post.CreatedByPersonGuid;
                requestPost.created = post.Created;

                foreach (var transaction in post.PayerTransactions)
                {
                    requestPost.payerTransactions.Add(new SyncPostsResponse.Transaction()
                    {
                        personGuid = transaction.PersonGuid,
                        amountSetManually = transaction.AmountSetManually,
                        displayname = transaction.Displayname,
                        isAmountSetManually = transaction.IsAmountSetManually
                    });
                }                
                
                foreach (var transaction in post.ConsumerTransactions)
                {
                    requestPost.consumerTransactions.Add(new SyncPostsResponse.Transaction()
                    {
                        personGuid = transaction.PersonGuid,
                        amountSetManually = transaction.AmountSetManually,
                        displayname = transaction.Displayname,
                        isAmountSetManually = transaction.IsAmountSetManually
                    });
                }
                requestPosts.Add(requestPost);
            }

            return requestPosts;
        }

        public List<SyncGroupModel> MapSyncGroupsRequestToSyncGroupModel(List<SyncGroupsRequest.Group> groups)
        {
            List<SyncGroupModel> syncGroupModels = new List<SyncGroupModel>();

            foreach (var group in groups)
            {
                var syncGroupModel = new SyncGroupModel();
                syncGroupModel.GroupGuid = group.groupGuid;
                syncGroupModel.IsDeleted = group.isDeleted;
                syncGroupModel.GroupGuid = group.groupGuid;
                syncGroupModel.CreatedByPersonGuid = group.createdByPersonGuid;
                syncGroupModel.Name = group.name;
                syncGroupModel.IsUsedInPostsOnApp = group.isInUseOnApp;

                syncGroupModels.Add(syncGroupModel);
            }
            return syncGroupModels;
        }

        public List<SyncGroupsResponse.Group> MapGroupToSyncGroupsResponseGroup(List<Group> groups)
        {
            List<SyncGroupsResponse.Group> requestGroups = new List<SyncGroupsResponse.Group>();

            foreach (var group in groups)
            {
                SyncGroupsResponse.Group requestGroup = new SyncGroupsResponse.Group();
                requestGroup.groupGuid = group.GroupGuid;
                requestGroup.isDeleted = group.IsDeleted;
                requestGroup.groupGuid = group.GroupGuid;
                requestGroup.createdByPersonGuid = group.CreatedByPersonGuid;
                requestGroup.name = group.Name;

                requestGroups.Add(requestGroup);
            }

            return requestGroups;
        }
    }

}