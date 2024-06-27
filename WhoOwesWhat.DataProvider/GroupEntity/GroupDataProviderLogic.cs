using System;
using WhoOwesWhat.Domain.DTO;

namespace WhoOwesWhat.DataProvider.GroupEntity
{
    public interface IGroupDataProviderLogic
    {
        Group MapToDomain(DataProvider.Entity.Group source);
    }

    public class GroupDataProviderLogic : IGroupDataProviderLogic
    {

        public Domain.DTO.Group MapToDomain(DataProvider.Entity.Group source)
        {
            Guard.NotNull(() => source, source);

            var targetDomain = new Domain.DTO.Group();
            targetDomain.GroupGuid = source.GroupGuid;
            targetDomain.Name = source.Name;
            targetDomain.IsDeleted = source.IsDeleted;
            targetDomain.CreatedByPersonGuid = source.CreatedBy.PersonGuid;
            return targetDomain;
        }
    }
}
