using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhoOwesWhat.Domain.DTO;

namespace WhoOwesWhat.DataProvider.Interfaces
{
    public interface IFriendrequestQuery
    {
        List<Friendrequest> GetFriendrequestsByRequester(Guid requesterPersonGuid);
        List<Friendrequest> GetFriendrequestsByPersonRequested(Guid personRequestedGuid);
        bool ExistsFriendrequest(Guid personRequestedGuid, Guid requesterPersonGuid);
    }

}
