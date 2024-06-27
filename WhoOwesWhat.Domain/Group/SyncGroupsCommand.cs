using System;
using System.Collections.Generic;
using System.Linq;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.Domain.DTO;
using WhoOwesWhat.Domain.Interfaces;

namespace WhoOwesWhat.Domain.Group
{
    public class SyncGroupsCommand : ISyncGroupsCommand
    {
        private readonly ISyncGroupCommand _syncGroupCommand;

        public SyncGroupsCommand(
            ISyncGroupCommand syncGroupCommand
            )
        {
            _syncGroupCommand = syncGroupCommand;
        }

        public void SyncGroups(string username, List<SyncGroupModel> syncGroupModels)
        {
            foreach (var syncGroupModel in syncGroupModels)
            {
                _syncGroupCommand.SyncGroup(username, syncGroupModel);
            }
        }
    }



}
