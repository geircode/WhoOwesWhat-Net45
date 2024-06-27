using System;
using System.Collections.Generic;
using System.Linq;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.Domain.DTO;
using WhoOwesWhat.Domain.Interfaces;

namespace WhoOwesWhat.Domain.Friendrequest
{
    public class FriendrequestRepository : IFriendrequestRepository
    {
        private readonly IPersonQuery _personQuery;
        private readonly IFriendQuery _friendQuery;
        private readonly IFriendrequestCommand _friendrequestCommand;
        private readonly IUserCredentialQuery _userCredentialQuery;
        private readonly IAcceptFriendrequestLogic _acceptFriendrequestLogic;
        private readonly IFriendrequestQuery _friendrequestQuery;

        public FriendrequestRepository(
            IPersonQuery personQuery,
            IFriendQuery friendQuery,
            IFriendrequestCommand friendrequestCommand,
            IUserCredentialQuery userCredentialQuery,
            IAcceptFriendrequestLogic acceptFriendrequestLogic,
            IFriendrequestQuery friendrequestQuery
            )
        {
            _personQuery = personQuery;
            _friendQuery = friendQuery;
            _friendrequestCommand = friendrequestCommand;
            _userCredentialQuery = userCredentialQuery;
            _acceptFriendrequestLogic = acceptFriendrequestLogic;
            _friendrequestQuery = friendrequestQuery;
        }

        public void SendFriendrequest(string username, string friendUsername)
        {
            Guard.NotNullOrEmpty(() => username, username);
            Guard.NotNullOrEmpty(() => friendUsername, friendUsername);

            // check for existing friend
            // save the friendrequest
            var requesterPerson = _personQuery.GetPersonByUsername(username);
            var personRequested = _personQuery.GetPersonByUsername(friendUsername);
            var friends = _friendQuery.GetFriends(requesterPerson.PersonGuid);
            if (friends.Any(a => a.FriendGuid == personRequested.PersonGuid))
            {
                throw new FriendrequestRepositoryException("Requested person is already a friend");
            }

            var existingFriendrequestsByRequesterPerson = _friendrequestQuery.GetFriendrequestsByRequester(requesterPerson.PersonGuid);
            if (existingFriendrequestsByRequesterPerson.Any(a => a.PersonRequestedGuid == personRequested.PersonGuid))
            {
                throw new FriendrequestRepositoryException("Requested person is already requested");
            }

            var existingFriendrequestsByPersonRequested = _friendrequestQuery.GetFriendrequestsByRequester(personRequested.PersonGuid);
            if (existingFriendrequestsByPersonRequested.Any(a => a.PersonRequestedGuid == requesterPerson.PersonGuid))
            {
                //throw new FriendrequestRepositoryException("Requested person has already sent you a friendrequest");
                _acceptFriendrequestLogic.AcceptFriendrequest(username, friendUsername);
                return;
            }

            if (requesterPerson.PersonGuid == personRequested.PersonGuid)
            {
                throw new FriendrequestRepositoryException("You can not request yourself as a Friend:/");
            }

            SaveFriendrequestModel friendrequest = new SaveFriendrequestModel()
            {
                RequesterPersonGuid = requesterPerson.PersonGuid,
                PersonRequestedGuid = personRequested.PersonGuid
            };

            _friendrequestCommand.SaveFriendrequest(friendrequest);
        }

        public List<UserCredential> GetFriendsrequestsToPerson(string username)
        {
            var person = _personQuery.GetPersonByUsername(username);

            var friendrequests = _friendrequestQuery.GetFriendrequestsByPersonRequested(person.PersonGuid);

            return
                friendrequests.Select(
                    friendrequest =>
                        _userCredentialQuery.GetUserCredentialByPersonGuid(friendrequest.RequesterPersonGuid)).ToList();
        }


        public class FriendrequestRepositoryException : Exception
        {
            public FriendrequestRepositoryException(string message)
                : base(message)
            {
            }
        }
    }



}
