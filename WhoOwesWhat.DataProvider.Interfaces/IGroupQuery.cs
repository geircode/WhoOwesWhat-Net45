using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhoOwesWhat.Domain.DTO;

namespace WhoOwesWhat.DataProvider.Interfaces
{
    public interface IGroupQuery
    {
        Domain.DTO.Group GetGroupByGroupGuid(Guid postGuid);
        List<Group> GetGroupsCreatedByAndUpdatedAfter(Guid personGuid, DateTime? lastSynchronizedToServer);
        List<Group> GetGroupsInUseByPersonGuidAndUpdatedAfter(Guid personGuid, DateTime value);
        List<Group> GetGroupsInUseByPersonGuid(Guid personGuid);
        bool IsGroupUsedInAnyPosts(Guid groupGuid);
    }
}
