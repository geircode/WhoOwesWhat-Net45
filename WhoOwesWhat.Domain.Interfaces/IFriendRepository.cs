using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhoOwesWhat.Domain.DTO;

namespace WhoOwesWhat.Domain.Interfaces
{
    public interface IFriendRepository
    {
        //void SyncOfflineFriend(string username, Guid personGuid, string displayname, bool isUsedInPostsOnApp, bool isFriendDeleted);
        void SaveOfflineFriend(string username, Guid friendGuid, string displayname);

        List<GetFriendsToAppModel> GetFriendsToApp(string username);

    }

    public interface IFriendRepositoryLogic
    {
        Friend MapToFriend(Guid friendGuid, Guid ownerGuid, bool isDeleted);
    }

    public interface IDeleteFriendCommand
    {
        void DeleteOnlineFriend(string username, Guid friendGuid);
        void UndeleteOnlineFriend(string username, Guid friendGuid);

        void DeleteOfflineFriend(string username, Guid friendGuid);
        void UndeleteOfflineFriend(string username, Guid friendGuid);
    }

}
