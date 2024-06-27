using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using log4net;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.Domain.DTO;

namespace WhoOwesWhat.DataProvider.FriendEntity
{
    public class FriendQuery : IFriendQuery
    {
        private readonly IWhoOwesWhatContext _whoOwesWhatContext;
        private ILog _log;
        private readonly IFriendDataProviderLogic _friendDataProviderLogic;

        public FriendQuery(IWhoOwesWhatContext whoOwesWhatContext, ILog log, IFriendDataProviderLogic friendDataProviderLogic)
        {
            _whoOwesWhatContext = whoOwesWhatContext;
            _log = log;
            _friendDataProviderLogic = friendDataProviderLogic;
        }

        public Friend GetFriend(Guid friendGuid, Guid ownerGuid)
        {
            var friendDb = _whoOwesWhatContext.GetFriendSqlRepository().GetAll().SingleOrDefault(a => a.Person.PersonGuid == friendGuid && a.Owner.PersonGuid == ownerGuid && !a.IsDeleted);
            return friendDb == null ? null : _friendDataProviderLogic.MapToDomain(friendDb, friendGuid, ownerGuid);
        }

        public List<Friend> GetFriendsIncludeDeleted(Guid ownerGuid)
        {
            List<Friend> friends = new List<Friend>();

            List<Entity.Friend> friendsDb = _whoOwesWhatContext.GetFriendSqlRepository().GetAll().Where(a => a.Owner.PersonGuid == ownerGuid).ToList();
            foreach (var friendDb in friendsDb)
            {
                _whoOwesWhatContext.LoadProperty(friendDb, a => a.Person);
                var friend = _friendDataProviderLogic.MapToDomain(friendDb, friendDb.Person.PersonGuid, ownerGuid);
                friends.Add(friend);
            }
            return friends;
        }


        public List<Friend> GetFriends(Guid ownerGuid)
        {
            List<Friend> friends = new List<Friend>();

            List<Entity.Friend> friendsDb = _whoOwesWhatContext.GetFriendSqlRepository().GetAll().Where(a => a.Owner.PersonGuid == ownerGuid && !a.IsDeleted).ToList();
            foreach (var friendDb in friendsDb)
            {
                _whoOwesWhatContext.LoadProperty(friendDb, a => a.Person);
                var friend = _friendDataProviderLogic.MapToDomain(friendDb, friendDb.Person.PersonGuid, ownerGuid);
                friends.Add(friend);
            }
            return friends;
        }


    }

}
