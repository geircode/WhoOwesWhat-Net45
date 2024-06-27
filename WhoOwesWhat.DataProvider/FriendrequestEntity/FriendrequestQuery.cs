using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.Domain.DTO;

namespace WhoOwesWhat.DataProvider.FriendrequestEntity
{
    public class FriendrequestQuery : IFriendrequestQuery
    {
        private readonly IWhoOwesWhatContext _whoOwesWhatContext;
        private ILog _log;

        public FriendrequestQuery(IWhoOwesWhatContext whoOwesWhatContext, ILog log)
        {
            _whoOwesWhatContext = whoOwesWhatContext;
            _log = log;
        }


        public List<Friendrequest> GetFriendrequestsByRequester(Guid requesterPersonGuid)
        {
            List<Friendrequest> friendrequests = new List<Friendrequest>();

            List<Entity.Friendrequest> friendRequestsDb = _whoOwesWhatContext.GetFriendrequestSqlRepository().GetAll().Where(a => a.RequesterPerson.PersonGuid == requesterPersonGuid).ToList();
            foreach (var friendRequestDb in friendRequestsDb)
            {
                _whoOwesWhatContext.Entry(friendRequestDb).Reference(p => p.PersonRequested).Load();
                var friendrequest = new Friendrequest()
                {
                    PersonRequestedGuid = friendRequestDb.PersonRequested.PersonGuid,
                    RequesterPersonGuid = requesterPersonGuid
                };
                friendrequests.Add(friendrequest);
            }
            return friendrequests;
        }

        public List<Friendrequest> GetFriendrequestsByPersonRequested(Guid personRequestedGuid)
        {
            List<Friendrequest> friendrequests = new List<Friendrequest>();

            List<Entity.Friendrequest> friendRequestsDb = _whoOwesWhatContext.GetFriendrequestSqlRepository().GetAll().Where(a => a.PersonRequested.PersonGuid == personRequestedGuid).ToList();
            foreach (var friendRequestDb in friendRequestsDb)
            {
                _whoOwesWhatContext.Entry(friendRequestDb).Reference(p => p.RequesterPerson).Load();
                var friendrequest = new Friendrequest()
                {
                    PersonRequestedGuid = personRequestedGuid,
                    RequesterPersonGuid = friendRequestDb.RequesterPerson.PersonGuid
                };
                friendrequests.Add(friendrequest);
            }
            return friendrequests;
        }

        public bool ExistsFriendrequest(Guid personRequestedGuid, Guid requesterPersonGuid)
        {
            Entity.Friendrequest friendRequestDb = _whoOwesWhatContext.GetFriendrequestSqlRepository()
                .GetAll()
                .SingleOrDefault(a => a.PersonRequested.PersonGuid == personRequestedGuid && a.RequesterPerson.PersonGuid == requesterPersonGuid);

            return friendRequestDb != null;
        }
    }
}
