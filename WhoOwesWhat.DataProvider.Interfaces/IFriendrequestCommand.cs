using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhoOwesWhat.Domain.DTO;

namespace WhoOwesWhat.DataProvider.Interfaces
{
    public interface IFriendrequestCommand
    {
        void SaveFriendrequest(Domain.DTO.SaveFriendrequestModel friendrequest);
        void DeleteFriendrequest(Guid personRequestedGuid, Guid requesterPersonGuid);
    }
}
