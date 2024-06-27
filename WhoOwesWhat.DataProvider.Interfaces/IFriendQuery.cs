using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhoOwesWhat.Domain.DTO;

namespace WhoOwesWhat.DataProvider.Interfaces
{
    public interface IFriendQuery
    {

        Friend GetFriend(Guid friendGuid, Guid ownerGuid);
        List<Friend> GetFriends(Guid ownerGuid);
        List<Friend> GetFriendsIncludeDeleted(Guid ownerGuid);
    }
}
