using System;
using System.Collections.Generic;
using System.Linq;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.Domain.DTO;
using WhoOwesWhat.Domain.Interfaces;

namespace WhoOwesWhat.Domain.Friendrequest
{
    public class AcceptFriendrequestLogic : IAcceptFriendrequestLogic
    {
        private readonly IPersonQuery _personQuery;
        private readonly IFriendQuery _friendQuery;
        private readonly IFriendrequestCommand _friendrequestCommand;
        private readonly IFriendCommand _friendCommand;
        private readonly IFriendrequestQuery _friendrequestQuery;

        public AcceptFriendrequestLogic(
            IPersonQuery personQuery,
            IFriendQuery friendQuery,
            IFriendrequestCommand friendrequestCommand,
            IFriendCommand friendCommand,
            IFriendrequestQuery friendrequestQuery
            )
        {
            _personQuery = personQuery;
            _friendQuery = friendQuery;
            _friendrequestCommand = friendrequestCommand;
            _friendCommand = friendCommand;
            _friendrequestQuery = friendrequestQuery;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username">The Person the friendrequest was initially sent to</param>
        /// <param name="friendUsername">The Person that sent the friendrequest that is now being accepted</param>
        public void AcceptFriendrequest(string username, string friendUsername)
        {
            Guard.NotNullOrEmpty(() => username, username);
            Guard.NotNullOrEmpty(() => friendUsername, friendUsername);

            // check for existing friend
            // save the friendrequest

            // personRequested => The Person the friendrequest was initially sent to
            // requesterPerson => The Person that sent the friendrequest that is now being accepted

            var personRequested = _personQuery.GetPersonByUsername(username);
            var requesterPerson = _personQuery.GetPersonByUsername(friendUsername);

            var friends = _friendQuery.GetFriends(personRequested.PersonGuid);
            if (friends.Any(a => a.FriendGuid == personRequested.PersonGuid))
            {
                throw new AcceptFriendrequestLogicException("Requested person is already a friend");
            }

            if (requesterPerson.PersonGuid == personRequested.PersonGuid)
            {
                throw new AcceptFriendrequestLogicException("You can not request yourself as a Friend:/");
            }

            var friendrequest = _friendrequestQuery.ExistsFriendrequest(personRequested.PersonGuid, requesterPerson.PersonGuid);
            if (!friendrequest)
            {
                throw new AcceptFriendrequestLogicException("Did not find friendrequest");
            }

            DTO.Friend newFriendship = new DTO.Friend()
            {
                FriendGuid = personRequested.PersonGuid,
                OwnerGuid = requesterPerson.PersonGuid
            };
            DTO.Friend newFriendshipInverse = new DTO.Friend()
            {
                FriendGuid = requesterPerson.PersonGuid,
                OwnerGuid = personRequested.PersonGuid
            };

            _friendCommand.SaveFriend(newFriendship);
            _friendCommand.SaveFriend(newFriendshipInverse);

            _friendrequestCommand.DeleteFriendrequest(personRequested.PersonGuid, requesterPerson.PersonGuid);
        }

        public class AcceptFriendrequestLogicException : Exception
        {
            public AcceptFriendrequestLogicException(string message)
                : base(message)
            {
            }
        }
    }



}
