using System;
using System.Collections.Generic;
using System.Linq;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.Domain.DTO;
using WhoOwesWhat.Domain.Interfaces;

namespace WhoOwesWhat.Domain.Group
{
    public class SyncGroupCommand : ISyncGroupCommand
    {
        private readonly IGroupQuery _groupQuery;
        private readonly IGroupCommand _groupCommand;
        private readonly IUserCredentialQuery _userCredentialQuery;
        private readonly ITransactionCommand _transactionCommand;

        public SyncGroupCommand(
            IGroupQuery groupQuery
            , IGroupCommand groupCommand
            , IUserCredentialQuery userCredentialQuery
            , ITransactionCommand transactionCommand
            )
        {
            _groupQuery = groupQuery;
            _groupCommand = groupCommand;
            _userCredentialQuery = userCredentialQuery;
            _transactionCommand = transactionCommand;
        }

        public void SyncGroup(string username, SyncGroupModel syncGroupModel)
        {
            // Create or update Group
            // On Update: Check if username has access to Group

            Guard.NotNullOrEmpty(() => username, username);
            Guard.NotNull(() => syncGroupModel, syncGroupModel);
            ValidateSyncGroupModel(syncGroupModel);

            var userPerson = _userCredentialQuery.GetUserCredential(username);

            // check for existing Group
            var group = _groupQuery.GetGroupByGroupGuid(syncGroupModel.GroupGuid);
            if (group != null && syncGroupModel.IsDeleted)
            {
                // if group exist, but the group is deleted on the App
                if (group.IsDeleted == false)
                {
                    // if the group is in use on the server, then ignore the deletion from the App
                    if (!_groupQuery.IsGroupUsedInAnyPosts(group.GroupGuid))
                    {
                        group.IsDeleted = true;
                        //_groupCommand.DeleteGroup(group.GroupGuid);
                    }
                }
                else
                {
                    throw new SyncGroupCommandException("Can not delete a Group that is already deleted");
                }
            }

            if (group != null && group.IsDeleted && syncGroupModel.IsDeleted == false && syncGroupModel.IsUsedInPostsOnApp)
            {
                // If the group is in use on the App, then it will have precedence over server
                // Undo the deletion of the Person and Group.
                group.IsDeleted = false;
                //_groupCommand.UnDeleteGroup(group.GroupGuid);
            }

            if (group == null)
            {
                group = new DTO.Group();
                group.GroupGuid = syncGroupModel.GroupGuid;
                group.CreatedByPersonGuid = syncGroupModel.CreatedByPersonGuid;
            }
            ValidateAuthorizationUsername(userPerson.Person.PersonGuid, group);

            group.Name = syncGroupModel.Name;

            _groupCommand.SaveGroup(group);
        }

        /// <summary>
        /// UserPerson needs to be ether Creator or part of the Transactions to be able to create or update a Group
        /// </summary>
        private void ValidateAuthorizationUsername(Guid userPersonGuid, DTO.Group model)
        {
            var isCreator = model.CreatedByPersonGuid == userPersonGuid;
            if (!isCreator)
            {
                throw new SyncGroupCommandException("UserPerson is not authorized to create or edit this Group: " + model.GroupGuid);
            }
        }


        private void ValidateSyncGroupModel(SyncGroupModel model)
        {
            Guard.NotNull(() => model.Name, model.Name);
            Guard.NotNull(() => model.GroupGuid, model.GroupGuid);
            Guard.NotNull(() => model.IsDeleted, model.IsDeleted);
            Guard.NotNull(() => model.CreatedByPersonGuid, model.CreatedByPersonGuid);
            Guard.IsValid(() => model.GroupGuid, model.GroupGuid, ValidatePersonGuid, "GroupGuid can not be empty");
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

        public class SyncGroupCommandException : Exception
        {
            public SyncGroupCommandException(string message)
                : base(message)
            {
            }
        }
    }
}
