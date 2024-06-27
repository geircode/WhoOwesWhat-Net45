using System;
using System.Linq;
using log4net;
using WhoOwesWhat.DataProvider.Entity;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.Domain.DTO;

namespace WhoOwesWhat.DataProvider.FriendEntity
{
    public class FriendCommand : IFriendCommand
    {
        private readonly IWhoOwesWhatContext _whoOwesWhatContext;
        private ILog _log;
        private readonly IFriendDataProviderLogic _friendDataProviderLogic;

        public FriendCommand(IWhoOwesWhatContext whoOwesWhatContext, ILog log, IFriendDataProviderLogic friendDataProviderLogic)
        {
            _whoOwesWhatContext = whoOwesWhatContext;
            _log = log;
            _friendDataProviderLogic = friendDataProviderLogic;
        }

        /// <summary>
        /// Will either create a new Friend or Undelete an existing friend
        /// </summary>
        /// <param name="friend"></param>
        public void SaveFriend(Domain.DTO.Friend friend)
        {
            var friendDb = _whoOwesWhatContext.GetFriendSqlRepository().GetAll().FirstOrDefault(a => a.Person.PersonGuid == friend.FriendGuid && a.Owner.PersonGuid == friend.OwnerGuid);

            if (friendDb == null)
            {
                var friendPerson = _whoOwesWhatContext.GetPersonSqlRepository().GetAll().Single(a => a.PersonGuid == friend.FriendGuid);
                var ownerPerson = _whoOwesWhatContext.GetPersonSqlRepository().GetAll().Single(a => a.PersonGuid == friend.OwnerGuid);

                friendDb = new Entity.Friend();
                friendDb.Person = friendPerson;
                friendDb.Owner = ownerPerson;
                _whoOwesWhatContext.GetFriendSqlRepository().Add(friendDb);
            }


            friendDb.IsDeleted = friend.IsDeleted;
            
            _whoOwesWhatContext.SaveChanges();
        }

        public void DeleteFriend(Guid friendGuid, Guid ownerGuid)
        {
            var friendDb = _whoOwesWhatContext.GetFriendSqlRepository().GetAll().FirstOrDefault(a => a.Person.PersonGuid == friendGuid && a.Owner.PersonGuid == ownerGuid);

            if (friendDb == null)
            {
                throw new FriendCommandException("Unable to find the Friend to delete");
            }

            friendDb.IsDeleted = true;

            
            _whoOwesWhatContext.SaveChanges();

            
        }

        public void UnDeleteFriend(Guid friendGuid, Guid ownerGuid)
        {
            var friendDb = _whoOwesWhatContext.GetFriendSqlRepository().GetAll().FirstOrDefault(a => a.Person.PersonGuid == friendGuid && a.Owner.PersonGuid == ownerGuid);

            if (friendDb == null)
            {
                throw new FriendCommandException("Unable to find the Friend to undelete");
            }

            friendDb.IsDeleted = false;


            _whoOwesWhatContext.SaveChanges();
        }

        public class FriendCommandException : Exception
        {
            public FriendCommandException(string message)
                : base(message)
            {
            }
        }
    }
}
