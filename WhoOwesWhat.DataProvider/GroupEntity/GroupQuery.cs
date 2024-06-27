using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using log4net;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.DataProvider.PostEntity;
using WhoOwesWhat.DataProvider.TransactionEntity;
using WhoOwesWhat.Domain.DTO;

namespace WhoOwesWhat.DataProvider.GroupEntity
{
    public class GroupQuery : IGroupQuery
    {
        private ILog _log;
        private readonly IGroupDataProviderLogic _groupDataProviderLogic;
        private readonly IGroupContext _groupContext;
        private readonly ITransactionContext _transactionContext;
        private readonly IPostContext _postContext;

        public GroupQuery(
            ILog log
            , IGroupDataProviderLogic groupDataProviderLogic
            , IGroupContext groupContext
            , ITransactionContext transactionContext
            , IPostContext postContext
            )
        {
            _log = log;
            _groupDataProviderLogic = groupDataProviderLogic;
            _groupContext = groupContext;
            _transactionContext = transactionContext;
            _postContext = postContext;
        }

        public Group GetGroupByGroupGuid(Guid groupGuid)
        {
            var groupDb = _groupContext.GetGroups().SingleOrDefault(a => a.GroupGuid == groupGuid && !a.IsDeleted);
            return groupDb == null ? null : _groupDataProviderLogic.MapToDomain(groupDb);
        }

        public List<Group> GetGroupsCreatedByAndUpdatedAfter(Guid personGuid, DateTime? lastSynchronizedToServer)
        {
            //Get all Posts created By personGuid
            var groupDb = _groupContext.GetGroups().Where(a => a.CreatedBy.PersonGuid == personGuid);
            if (lastSynchronizedToServer.HasValue)
            {
                groupDb = groupDb.Where(a => a.VersionUpdated > lastSynchronizedToServer);
            }

            foreach (var group in groupDb)
            {
                _groupContext.WhoOwesWhatContext.LoadProperty(group, a => a.CreatedBy);
            }

            return groupDb.Select(_groupDataProviderLogic.MapToDomain).ToList();
        }

        public List<Group> GetGroupsInUseByPersonGuidAndUpdatedAfter(Guid personGuid, DateTime lastSynchronizedToServer)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            var groupsInUse = _transactionContext.GetTransactions().Where(
                b => b.Post.VersionUpdated > lastSynchronizedToServer
                    && b.Person.PersonGuid == personGuid
                    && b.Post.IsDeleted == false
                    && b.Post.Group != null
                    )
                    .Select(b => b.Post.Group).Distinct();

            foreach (var group in groupsInUse)
            {
                _groupContext.WhoOwesWhatContext.LoadProperty(group, a => a.CreatedBy);
            }

            stopwatch.Stop();
            Console.WriteLine("GetGroupsInUseByPersonGuidAndUpdatedAfter: " + stopwatch.ElapsedMilliseconds + " ms");

            return groupsInUse.Select(_groupDataProviderLogic.MapToDomain).ToList();
        }

        public List<Group> GetGroupsInUseByPersonGuid(Guid personGuid)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            var groupsInUse = _transactionContext.GetTransactions().Where(
                b => b.Person.PersonGuid == personGuid
                    && b.Post.IsDeleted == false
                    && b.Post.Group != null
                    )
                    .Select(b => b.Post.Group);

            foreach (var group in groupsInUse)
            {
                _groupContext.WhoOwesWhatContext.LoadProperty(group, a => a.CreatedBy);
            }


            stopwatch.Stop();
            Console.WriteLine("GetGroupsInUseByPersonGuid: " + stopwatch.ElapsedMilliseconds + " ms");

            return groupsInUse.Select(_groupDataProviderLogic.MapToDomain).ToList();
        }

        public bool IsGroupUsedInAnyPosts(Guid groupGuid)
        {
            return _postContext.GetPosts().Any(a => a.Group.GroupGuid == groupGuid);
        }
    }

}
