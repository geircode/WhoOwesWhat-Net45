using System;
using WhoOwesWhat.Domain.DTO;

namespace WhoOwesWhat.DataProvider.Interfaces
{
    public interface IFriendCommand
    {
        void SaveFriend(Domain.DTO.Friend friend);
        void DeleteFriend(Guid friendGuid, Guid ownerGuid);
        void UnDeleteFriend(Guid friendGuid, Guid ownerGuid);
    }
}