using System;
using WhoOwesWhat.Domain.DTO;

namespace WhoOwesWhat.DataProvider.FriendEntity
{
    public interface IFriendDataProviderLogic
    {
        Friend MapToDomain(DataProvider.Entity.Friend source, Guid friendGuid, Guid ownerGuid);
    }

    public class FriendDataProviderLogic : IFriendDataProviderLogic
    {

        public Domain.DTO.Friend MapToDomain(DataProvider.Entity.Friend source, Guid friendGuid, Guid ownerGuid)
        {
            Guard.NotNull(() => source, source);

            var targetDomain = new Domain.DTO.Friend();
            targetDomain.IsDeleted = source.IsDeleted;
            targetDomain.OwnerGuid = ownerGuid;
            targetDomain.FriendGuid = friendGuid;
            targetDomain.IsDeleted = source.IsDeleted;
            return targetDomain;
        }
    }
}
