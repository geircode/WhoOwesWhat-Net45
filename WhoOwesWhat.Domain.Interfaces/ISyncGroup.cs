using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhoOwesWhat.Domain.DTO;

namespace WhoOwesWhat.Domain.Interfaces
{
    public interface ISyncGroupsCommand
    {
        void SyncGroups(string username, List<SyncGroupModel> syncGroupModels);
    }     
    
    public interface ISyncGroupCommand
    {
        void SyncGroup(string username, SyncGroupModel syncGroupModel);
    }

    public interface ISyncGroupQuery
    {
        List<Group> GetUnsynchronizedGroups(string username, DateTime? lastSynchronizedToServer);
    }


    public interface IDeleteGroupLogic
    {
        void DeleteGroup(Guid groupGuid, Guid ownerGuid);
        void UnDeleteGroup(Guid groupGuid, Guid ownerGuid);
    }

}
