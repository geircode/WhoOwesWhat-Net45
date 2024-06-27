using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using log4net;
using WhoOwesWhat.DataProvider.Entity;
using WhoOwesWhat.DataProvider.FriendEntity;
using WhoOwesWhat.DataProvider.Interfaces;

namespace WhoOwesWhat.DataProvider.FriendrequestEntity
{
    public class FriendrequestCommand : IFriendrequestCommand
    {
        private readonly IWhoOwesWhatContext _whoOwesWhatContext;
        private ILog _log;

        public FriendrequestCommand(IWhoOwesWhatContext whoOwesWhatContext, ILog log)
        {
            _whoOwesWhatContext = whoOwesWhatContext;
            _log = log;
        }

        public void SaveFriendrequest(Domain.DTO.SaveFriendrequestModel friendrequest)
        {
            var personRequested = _whoOwesWhatContext.GetPersonSqlRepository().GetAll().Single(a => a.PersonGuid == friendrequest.PersonRequestedGuid);
            var requesterPerson = _whoOwesWhatContext.GetPersonSqlRepository().GetAll().Single(a => a.PersonGuid == friendrequest.RequesterPersonGuid);

            var friendRequestDb = _whoOwesWhatContext.GetFriendrequestSqlRepository().GetAll().FirstOrDefault(a => a.PersonRequested.PersonGuid == friendrequest.PersonRequestedGuid && a.RequesterPerson.PersonGuid == friendrequest.RequesterPersonGuid);

            if (friendRequestDb == null)
            {
                friendRequestDb = new Friendrequest();
                _whoOwesWhatContext.GetFriendrequestSqlRepository().Add(friendRequestDb);
            }
            else
            {
                throw new FriendrequestCommandException("Possible bug: A friendrequest is read-only. Unable to change an existing friendrequest");
            }

            friendRequestDb.PersonRequested = personRequested;
            friendRequestDb.RequesterPerson = requesterPerson;
            
            _whoOwesWhatContext.SaveChanges();
        }

        public void DeleteFriendrequest(Guid personRequestedGuid, Guid requesterPersonGuid)
        {
            var friendrequestDb = _whoOwesWhatContext.GetFriendrequestSqlRepository().GetAll().FirstOrDefault(a => a.PersonRequested.PersonGuid == personRequestedGuid && a.RequesterPerson.PersonGuid == requesterPersonGuid);

            if (friendrequestDb == null)
            {
                throw new FriendrequestCommandException("Unable to find the Friendrequest to delete");
            }

            _whoOwesWhatContext.GetFriendrequestSqlRepository().Remove(friendrequestDb);
            _whoOwesWhatContext.SaveChanges();
        }


        public class FriendrequestCommandException : Exception
        {
            public FriendrequestCommandException(string message)
                : base(message)
            {
            }
        }
    }
}
