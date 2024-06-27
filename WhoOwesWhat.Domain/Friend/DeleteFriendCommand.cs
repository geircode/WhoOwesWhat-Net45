using System;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.Domain.DTO;
using WhoOwesWhat.Domain.Interfaces;

namespace WhoOwesWhat.Domain.Friend
{
    public class DeleteFriendCommand : IDeleteFriendCommand
    {
        private readonly IUserCredentialQuery _credentialQuery;
        private readonly ITransactionQuery _transactionQuery;
        private readonly IPersonCommand _personCommand;
        private readonly IFriendCommand _friendCommand;
        private readonly IPersonQuery _personQuery;
        private readonly IFriendQuery _friendQuery;

        public DeleteFriendCommand(IUserCredentialQuery credentialQuery
            , ITransactionQuery transactionQuery
            , IPersonCommand personCommand
            , IFriendCommand friendCommand
            , IPersonQuery personQuery
            , IFriendQuery friendQuery
            )
        {
            _credentialQuery = credentialQuery;
            _transactionQuery = transactionQuery;
            _personCommand = personCommand;
            _friendCommand = friendCommand;
            _personQuery = personQuery;
            _friendQuery = friendQuery;
        }

        public void DeleteOfflineFriend(string username, Guid friendGuid)
        {
            Guard.NotNullOrEmpty(() => username, username);
            Guard.NotNull(() => friendGuid, friendGuid);
            Guard.IsValid(() => friendGuid, friendGuid, ValidatePersonGuid, "PersonGuid can not be empty");
            Guard.IsValid(() => friendGuid, friendGuid, _credentialQuery.IsNotOnlinePerson, "Only OfflineFriends can be synchronized with this function");

            var ownerPerson = _personQuery.GetPersonByUsername(username);
            if (ownerPerson == null)
            {
                throw new Exception("Unable to GetPersonByUsername");
            }

            // check for existing Friend
            var friend = _friendQuery.GetFriend(friendGuid, ownerPerson.PersonGuid);
            if (friend == null)
            {
                throw new DeleteOfflineFriendLogicException("Could not find friend. Unable to delete OfflineFriend. ");
            }

            if (_transactionQuery.IsPersonUsedInAnyPosts(friendGuid))
            {
                throw new DeleteOfflineFriendLogicException("Unable to delete. Friend is already in use in one or more Posts.");
            }

            _personCommand.DeletePerson(friend.FriendGuid);
            _friendCommand.DeleteFriend(friend.FriendGuid, friend.OwnerGuid);
            
        }

        public void UndeleteOfflineFriend(string username, Guid friendGuid)
        {
            Guard.NotNullOrEmpty(() => username, username);
            Guard.NotNull(() => friendGuid, friendGuid);
            Guard.IsValid(() => friendGuid, friendGuid, ValidatePersonGuid, "PersonGuid can not be empty");
            Guard.IsValid(() => friendGuid, friendGuid, _credentialQuery.IsNotOnlinePerson, "Only OfflineFriends can be synchronized with this function");

            var ownerPerson = _personQuery.GetPersonByUsername(username);
            if (ownerPerson == null)
            {
                throw new Exception("Unable to GetPersonByUsername");
            }

            // check for existing Friend
            var friend = _friendQuery.GetFriend(friendGuid, ownerPerson.PersonGuid);
            if (friend == null)
            {
                throw new DeleteOfflineFriendLogicException("Could not find friend. Unable to delete OfflineFriend. ");
            }

            if (_transactionQuery.IsPersonUsedInAnyPosts(friendGuid))
            {
                throw new DeleteOfflineFriendLogicException("Unable to delete. Friend is already in use in one or more Posts.");
            }

            _personCommand.UnDeletePerson(friendGuid);
            _friendCommand.UnDeleteFriend(friend.FriendGuid, friend.OwnerGuid);
        }

        public void DeleteOnlineFriend(string username, Guid friendGuid)
        {
            Guard.NotNullOrEmpty(() => username, username);
            Guard.NotNull(() => friendGuid, friendGuid);
            Guard.IsValid(() => friendGuid, friendGuid, ValidatePersonGuid, "PersonGuid can not be empty");
            Guard.IsValid(() => friendGuid, friendGuid, _credentialQuery.IsOnlinePerson, "Only OnlinePersons can be synchronized with this function");

            if (_transactionQuery.IsPersonUsedInAnyPosts(friendGuid))
            {
                throw new DeleteOfflineFriendLogicException("Unable to delete. Friend is already in use in one or more Posts.");
            }

            var ownerPerson = _personQuery.GetPersonByUsername(username);
            if (ownerPerson == null)
            {
                throw new Exception("Unable to GetPersonByUsername");
            }

            // check for existing Friend
            var friend = _friendQuery.GetFriend(friendGuid, ownerPerson.PersonGuid);
            if (friend == null)
            {
                throw new DeleteOfflineFriendLogicException("OnlineFriend not found. Can not delete Friend.");
            }

            _friendCommand.DeleteFriend(friendGuid, friend.OwnerGuid);
            _friendCommand.DeleteFriend(friend.OwnerGuid, friendGuid);
        }

        public void UndeleteOnlineFriend(string username, Guid friendGuid)
        {
            Guard.NotNullOrEmpty(() => username, username);
            Guard.NotNull(() => friendGuid, friendGuid);
            Guard.IsValid(() => friendGuid, friendGuid, ValidatePersonGuid, "PersonGuid can not be empty");
            Guard.IsValid(() => friendGuid, friendGuid, _credentialQuery.IsOnlinePerson, "Only OnlinePersons can be synchronized with this function");

            if (_transactionQuery.IsPersonUsedInAnyPosts(friendGuid))
            {
                throw new DeleteOfflineFriendLogicException("Unable to delete. Friend is already in use in one or more Posts.");
            }

            var ownerPerson = _personQuery.GetPersonByUsername(username);
            if (ownerPerson == null)
            {
                throw new Exception("Unable to GetPersonByUsername");
            }

            // check for existing Friend
            var friend = _friendQuery.GetFriend(friendGuid, ownerPerson.PersonGuid);
            if (friend == null)
            {
                throw new DeleteOfflineFriendLogicException("OnlineFriend not found. Can not delete Friend.");
            }

            _friendCommand.UnDeleteFriend(friendGuid, friend.OwnerGuid);
            _friendCommand.UnDeleteFriend(friend.OwnerGuid, friendGuid);
        }


        private bool ValidatePersonGuid(Guid guid)
        {
            return guid != Guid.Empty;
        }

        private bool ValidateIsTrue(bool istrue)
        {
            return istrue == true;
        }
    }

    public class DeleteOfflineFriendLogicException : Exception
    {
        public DeleteOfflineFriendLogicException(string message)
            : base(message)
        {
        }
    }



}
