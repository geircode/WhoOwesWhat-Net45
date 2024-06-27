using System;
using System.Collections.Generic;
using System.Linq;
using WhoOwesWhat.DataProvider.Entity;

namespace WhoOwesWhat.DataProvider.GroupEntity
{
    public interface IGroupContext : IContextBase
    {
        Entity.Group GetGroup(int groupId);
        Entity.Group GetGroupByGroupGuid(Guid groupGuid);
        IQueryable<Group> GetGroups();
    }

    public class GroupContext : IGroupContext
    {
        private readonly IWhoOwesWhatContext _whoOwesWhatContext;

        public GroupContext(IWhoOwesWhatContext whoOwesWhatContext)
        {
            _whoOwesWhatContext = whoOwesWhatContext;
        }

        public Group GetGroup(int groupId)
        {
            return _whoOwesWhatContext.GetGroupSqlRepository().GetAll().SingleOrDefault(a => a.GroupId == groupId);
        }

        public Group GetGroupByGroupGuid(Guid groupGuid)
        {
            return _whoOwesWhatContext.GetGroupSqlRepository().GetAll().SingleOrDefault(a => a.GroupGuid == groupGuid);
        }

        public IQueryable<Group> GetGroups()
        {
            return _whoOwesWhatContext.GetGroupSqlRepository().GetAll();
        }

        public IWhoOwesWhatContext WhoOwesWhatContext
        {
            get { return _whoOwesWhatContext; }
        }
    }
}
