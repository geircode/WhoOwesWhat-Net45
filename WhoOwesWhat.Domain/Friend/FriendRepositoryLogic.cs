using System;
using WhoOwesWhat.Domain.Interfaces;

namespace WhoOwesWhat.Domain.Friend
{
    public class FriendRepositoryLogic : IFriendRepositoryLogic
    {
        public DTO.Friend MapToFriend(Guid friendGuid, Guid ownerGuid, bool isDeleted)
        {
            var user = new Domain.DTO.Friend()
            {
                FriendGuid = friendGuid,
                OwnerGuid = ownerGuid,
                IsDeleted = isDeleted,
            };

            return user;
        }        
        
    }
}
