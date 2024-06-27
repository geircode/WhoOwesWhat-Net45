using System;
using System.Collections.Generic;
using System.Linq;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.Domain.DTO;
using WhoOwesWhat.Domain.Interfaces;

namespace WhoOwesWhat.Domain.Group
{
    public class SyncGroupQuery : ISyncGroupQuery
    {
        private readonly IGroupQuery _groupQuery;
        private readonly IUserCredentialQuery _userCredentialQuery;
        private readonly ITransactionQuery _transactionQuery;

        public SyncGroupQuery(
            IGroupQuery groupQuery
            , IUserCredentialQuery userCredentialQuery
            , ITransactionQuery transactionQuery
            )
        {
            _groupQuery = groupQuery;
            _userCredentialQuery = userCredentialQuery;
            _transactionQuery = transactionQuery;
        }


        public class SyncGroupQueryException : Exception
        {
            public SyncGroupQueryException(string message)
                : base(message)
            {
            }
        }


        public List<DTO.Group> GetUnsynchronizedGroups(string username, DateTime? lastSynchronizedToServer)
        {
            // Get all Groups created By personGuid
            // Get all Groups where personGuid is used in a Transactions 
            var user = _userCredentialQuery.GetUserCredential(username);

            var groupsToApp = new List<DTO.Group>();

            List<DTO.Group> groupsCreatedBy = _groupQuery.GetGroupsCreatedByAndUpdatedAfter(user.Person.PersonGuid, lastSynchronizedToServer);
            groupsToApp.AddRange(groupsCreatedBy);
            List<DTO.Group> groupsInUseBy;
            if (lastSynchronizedToServer.HasValue)
            {
                groupsInUseBy = _groupQuery.GetGroupsInUseByPersonGuidAndUpdatedAfter(user.Person.PersonGuid, lastSynchronizedToServer.Value);
            }
            else
            {
                groupsInUseBy = _groupQuery.GetGroupsInUseByPersonGuid(user.Person.PersonGuid);
            }
            groupsToApp.AddRange(groupsInUseBy);
            var comparer = new GroupComparer();
            groupsToApp = groupsToApp.Distinct(comparer).ToList();

            return groupsToApp;

        }

        public class GroupComparer : IEqualityComparer<DTO.Group>
        {
            public bool Equals(DTO.Group x, DTO.Group y)
            {
                return x.GroupGuid == y.GroupGuid;
            }

            public int GetHashCode(DTO.Group obj)
            {
                return 1;
            }
        }
    }




}
