using System;
using System.Collections.Generic;
using WhoOwesWhat.Domain.DTO;
using WhoOwesWhat.Service.DTO;

namespace WhoOwesWhat.Tests
{

    public interface ITestPostQuery
    {
        List<SyncPostsRequest.Post> GetSyncPostsRequestPost_Hooters();  
        SyncPostModel GetSyncPostModel_Hooters();
        DataProvider.Entity.Post GetEntityPost_Hooters();
        Domain.DTO.Post GetDomainPost_Hooters();
        Domain.DTO.Post GetDomainPost_Kino();
    }

    public class TestPostQuery : ITestPostQuery
    {
        public List<SyncPostsRequest.Post> GetSyncPostsRequestPost_Hooters()
        {
            Person ownerPerson = TestPersonQuery.GeirPersonDomain;
            Person victorPerson = TestPersonQuery.VictorPersonDomain;

            var postHooters = new SyncPostsRequest.Post()
            {
                postGuid = new Guid("ec8f8ded-b988-f4bc-4c8c-ef9c956c5891"),
                purchaseDate = new DateTime(2012, 03, 20),
                description = "Hooters",
                totalCost = "12300",
                iso4217CurrencyCode = "NOK",
                version = 0,
                isDeleted = false,
                comment = null,
                groupGuid = null,
                lastUpdatedByPersonGuid = ownerPerson.PersonGuid,
                lastUpdated = new DateTime(2012, 03, 20),
                createdByPersonGuid = ownerPerson.PersonGuid,
                created = new DateTime(2012, 03, 20),
            };
            postHooters.payerTransactions.Add(new SyncPostsRequest.Transaction()
            {
                personGuid = victorPerson.PersonGuid,
                isAmountSetManually = false,
                amountSetManually = null
            });
            postHooters.consumerTransactions.Add(new SyncPostsRequest.Transaction()
            {
                personGuid = ownerPerson.PersonGuid,
                isAmountSetManually = false,
                amountSetManually = null
            });
            postHooters.consumerTransactions.Add(new SyncPostsRequest.Transaction()
            {
                personGuid = victorPerson.PersonGuid,
                isAmountSetManually = false,
                amountSetManually = null
            });

            var posts = new List<SyncPostsRequest.Post>();
            posts.Add(postHooters);

            return posts;
        }    
        public SyncPostModel GetSyncPostModel_Hooters()
        {
            Person ownerPerson = TestPersonQuery.GeirPersonDomain;
            Person victorPerson = TestPersonQuery.VictorPersonDomain;
          
            var postHooters = new SyncPostModel()
            {
                PostGuid = new Guid("ec8f8ded-b988-f4bc-4c8c-ef9c956c5891"),
                PurchaseDate = new DateTime(2012, 03, 20),
                Description = "Hooters",
                TotalCost = "12300",
                Iso4217CurrencyCode = "NOK",
                Version = 0,
                IsDeleted = false,
                Comment = null,
                GroupGuid = null,
                LastUpdatedByPersonGuid = ownerPerson.PersonGuid,
                LastUpdated = new DateTime(2012, 03, 20),
                CreatedByPersonGuid = ownerPerson.PersonGuid,
                Created = new DateTime(2012, 03, 20),
            };
            postHooters.Payers.Add(new SyncPostModel.SyncPayerModel()
            {
                PersonGuid = victorPerson.PersonGuid,
                IsAmountSetManually = false,
                Amount = null
            });
            postHooters.Consumers.Add(new SyncPostModel.SyncConsumerModel()
            {
                PersonGuid = ownerPerson.PersonGuid,
                IsAmountSetManually = false,
                Amount = null
            });
            postHooters.Consumers.Add(new SyncPostModel.SyncConsumerModel()
            {
                PersonGuid = victorPerson.PersonGuid,
                IsAmountSetManually = false,
                Amount = null
            });

            return postHooters;
        }

        public Domain.DTO.Post GetDomainPost_Hooters()
        {
            var ownerPerson = TestPersonQuery.GeirPersonEntity;

            var postHooters = new Domain.DTO.Post()
            {
                PostGuid = new Guid("ec8f8ded-b988-f4bc-4c8c-ef9c956c5891"),
                PurchaseDate = new DateTime(2012, 03, 20),
                Description = "Hooters",
                TotalCost = "12300",
                Iso4217CurrencyCode = "NOK",
                Version = 0,
                IsDeleted = false,
                Comment = null,
                GroupGuid = null,
                LastUpdatedByPersonGuid = ownerPerson.PersonGuid,
                LastUpdated = new DateTime(2012, 03, 20),
                CreatedByPersonGuid = ownerPerson.PersonGuid,
                Created = new DateTime(2012, 03, 20),
            };

            //postHooters.Payers.Add(new SyncPostModel.SyncPayerModel()
            //{
            //    PersonGuid = victorPerson.PersonGuid,
            //    IsAmountSetManually = false,
            //    AmountSetManually = null
            //});
            //postHooters.Consumers.Add(new SyncPostModel.SyncConsumerModel()
            //{
            //    PersonGuid = ownerPerson.PersonGuid,
            //    IsAmountSetManually = false,
            //    AmountSetManually = null
            //});
            //postHooters.Consumers.Add(new SyncPostModel.SyncConsumerModel()
            //{
            //    PersonGuid = victorPerson.PersonGuid,
            //    IsAmountSetManually = false,
            //    AmountSetManually = null
            //});

            return postHooters;
        }

        public Post GetDomainPost_Kino()
        {
            var ownerPerson = TestPersonQuery.BeatePersonDomain;

            var post = new Domain.DTO.Post()
            {
                PostGuid = new Guid("27916d3d-a638-ddbc-4ce9-765dea191cc7"),
                PurchaseDate = new DateTime(2008, 12, 18),
                Description = "Kino",
                TotalCost = "50000",
                Iso4217CurrencyCode = "NOK",
                Version = 1,
                IsDeleted = false,
                Comment = null,
                GroupGuid = null,
                LastUpdatedByPersonGuid = ownerPerson.PersonGuid,
                LastUpdated = new DateTime(2012, 03, 20),
                CreatedByPersonGuid = ownerPerson.PersonGuid,
                Created = new DateTime(2012, 03, 20),
            };

            return post;
        }

        public DataProvider.Entity.Post GetEntityPost_Hooters()
        {
            var ownerPerson = TestPersonQuery.GeirPersonEntity;

            var postHooters = new DataProvider.Entity.Post()
            {
                PostId = 1337,
                PostGuid = new Guid("ec8f8ded-b988-f4bc-4c8c-ef9c956c5891"),
                PurchaseDate = new DateTime(2012, 03, 20),
                Description = "Hooters",
                TotalCost = "12300",
                Iso4217CurrencyCode = "NOK",
                Version = 0,
                IsDeleted = false,
                Comment = null,
                Group = null,
                LastUpdatedBy = ownerPerson,
                LastUpdated = new DateTime(2012, 03, 20),
                CreatedBy = ownerPerson,
                Created = new DateTime(2012, 03, 20),
            };

            //postHooters.Payers.Add(new SyncPostModel.SyncPayerModel()
            //{
            //    PersonGuid = victorPerson.PersonGuid,
            //    IsAmountSetManually = false,
            //    AmountSetManually = null
            //});
            //postHooters.Consumers.Add(new SyncPostModel.SyncConsumerModel()
            //{
            //    PersonGuid = ownerPerson.PersonGuid,
            //    IsAmountSetManually = false,
            //    AmountSetManually = null
            //});
            //postHooters.Consumers.Add(new SyncPostModel.SyncConsumerModel()
            //{
            //    PersonGuid = victorPerson.PersonGuid,
            //    IsAmountSetManually = false,
            //    AmountSetManually = null
            //});

            return postHooters;
        }
    }
}
