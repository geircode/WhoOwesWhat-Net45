using System;
using System.Linq;
using log4net;
using WhoOwesWhat.DataProvider.Entity;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.DataProvider.PersonEntity;
using WhoOwesWhat.Domain.DTO;

namespace WhoOwesWhat.DataProvider.GroupEntity
{
    public class GroupCommand : IGroupCommand
    {
        private readonly IWhoOwesWhatContext _whoOwesWhatContext;
        private ILog _log;
        private readonly IGroupDataProviderLogic _groupDataProviderLogic;
        private readonly IGroupContext _groupContext;
        private readonly IPersonContext _personContext;

        public GroupCommand(
            IWhoOwesWhatContext whoOwesWhatContext
            , ILog log
            , IGroupDataProviderLogic groupDataProviderLogic
            , IGroupContext groupContext
            , IPersonContext personContext
            )
        {
            _whoOwesWhatContext = whoOwesWhatContext;
            _log = log;
            _groupDataProviderLogic = groupDataProviderLogic;
            _groupContext = groupContext;
            _personContext = personContext;
        }

        /// <summary>
        /// Will either create a new Group or Undelete an existing group
        /// </summary>
        /// <param name="group"></param>
        public void SaveGroup(Domain.DTO.Group group)
        {
            var groupDb = _groupContext.GetGroupByGroupGuid(group.GroupGuid);

            if (groupDb == null)
            {
                groupDb = new Entity.Group();
                groupDb.CreatedBy = _personContext.GetPersonByPersonGuid(group.CreatedByPersonGuid);
                groupDb.GroupGuid = group.GroupGuid;

                _whoOwesWhatContext.GetGroupSqlRepository().Add(groupDb);
            }

            groupDb.VersionUpdated = DateTime.Now;
            groupDb.Name = group.Name;
            groupDb.IsDeleted = group.IsDeleted;

            _whoOwesWhatContext.SaveChanges();
        }

        public void DeleteGroup(Guid groupGuid)
        {
            var groupDb = _groupContext.GetGroupByGroupGuid(groupGuid);

            if (groupDb == null)
            {
                throw new GroupCommandException("Unable to find the Group to delete");
            }

            groupDb.IsDeleted = true;


            _whoOwesWhatContext.SaveChanges();


        }

        public void UnDeleteGroup(Guid groupGuid)
        {
            var groupDb = _groupContext.GetGroupByGroupGuid(groupGuid);

            if (groupDb == null)
            {
                throw new GroupCommandException("Unable to find the Group to undelete");
            }

            groupDb.IsDeleted = false;


            _whoOwesWhatContext.SaveChanges();
        }

        public class GroupCommandException : Exception
        {
            public GroupCommandException(string message)
                : base(message)
            {
            }
        }
    }
}
