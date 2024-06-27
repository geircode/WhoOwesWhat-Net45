using System;
using System.Collections.Generic;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.Domain.DTO;
using WhoOwesWhat.Domain.Interfaces;

namespace WhoOwesWhat.Domain.Friend
{
    public class FriendRepository : IFriendRepository
    {
        private readonly IPersonQuery _personQuery;
        private readonly IFriendCommand _friendCommand;
        private readonly IPersonCommand _personCommand;
        private readonly IUserCredentialQuery _credentialQuery;
        private readonly IFriendQuery _friendQuery;
        private readonly IFriendRepositoryLogic _friendRepositoryLogic;
        private readonly IDeleteFriendCommand _deleteFriend;
        private readonly ITransactionQuery _transactionQuery;

        public FriendRepository(IPersonQuery personQuery
            , IFriendCommand friendCommand
            , IPersonCommand personCommand
            , IUserCredentialQuery credentialQuery
            , IFriendQuery friendQuery
            , IFriendRepositoryLogic friendRepositoryLogic
            , IDeleteFriendCommand deleteFriend
            , ITransactionQuery transactionQuery)
        {
            _personQuery = personQuery;
            _friendCommand = friendCommand;
            _personCommand = personCommand;
            _credentialQuery = credentialQuery;
            _friendQuery = friendQuery;
            _friendRepositoryLogic = friendRepositoryLogic;
            _deleteFriend = deleteFriend;
            _transactionQuery = transactionQuery;
        }

       

        public void SaveOfflineFriend(string username, Guid friendGuid, string displayname)
        {
            Guard.NotNullOrEmpty(() => username, username);
            Guard.NotNull(() => friendGuid, friendGuid);
            Guard.NotNullOrEmpty(() => displayname, displayname);
            Guard.IsValid(() => friendGuid, friendGuid, ValidatePersonGuid, "PersonGuid can not be empty");
            Guard.IsValid(() => friendGuid, friendGuid, _credentialQuery.IsNotOnlinePerson, "Only OfflineFriends can be synchronized with this function");

            var ownerPerson = _personQuery.GetPersonByUsername(username);
            if (ownerPerson == null)
            {
                throw new Exception("Unable to GetPersonByUsername");
            }

            // check for existing Friend
            var friend = _friendQuery.GetFriend(friendGuid, ownerPerson.PersonGuid);
            if (friend != null)
            {
                var friendPerson = _personQuery.GetPerson(friendGuid);
                if (friendPerson == null)
                {
                    throw new Exception("Unable to GetPerson");
                }
                friendPerson.Displayname = displayname;
                _personCommand.SavePerson(friendPerson);

            }
            else
            {
                var friendPerson = new Person();
                friendPerson.PersonGuid = friendGuid;
                friendPerson.Displayname = displayname;
                _personCommand.SavePerson(friendPerson);

                friend = new Domain.DTO.Friend()
                {
                    FriendGuid = friendGuid,
                    OwnerGuid = ownerPerson.PersonGuid,
                };

                _friendCommand.SaveFriend(friend);
            }
        }

        
        public List<GetFriendsToAppModel> GetFriendsToApp(string username)
        {
            List<GetFriendsToAppModel> modelList = new List<GetFriendsToAppModel>();

            var ownerPerson = _personQuery.GetPersonByUsername(username);

            var friends = _friendQuery.GetFriendsIncludeDeleted(ownerPerson.PersonGuid);

            foreach (var friend in friends)
            {

                var model = new GetFriendsToAppModel();

                model.FriendGuid = friend.FriendGuid;
                model.OwnerGuid = friend.OwnerGuid;
                model.IsFriendDeleted = friend.IsDeleted;

                var friendPerson = _personQuery.GetPerson(friend.FriendGuid);
                model.Displayname = friendPerson.Displayname;
                model.IsPersonDeleted = friendPerson.IsDeleted;

                model.IsFriendOnlinePerson = _credentialQuery.IsOnlinePerson(friendPerson.PersonGuid);

                modelList.Add(model);
            }

            return modelList;
        }
        private bool ValidatePersonGuid(Guid guid)
        {
            return guid != Guid.Empty;
        }

        private bool ValidateIsTrue(bool istrue)
        {
            return istrue == true;
        }

        public class FriendRepositoryException : Exception
        {
            public FriendRepositoryException(string message)
                : base(message)
            {
            }
        }

    }



}
