using System;
using WhoOwesWhat.Domain.DTO;

namespace WhoOwesWhat.DataProvider.Interfaces
{
    public interface IGroupCommand
    {
        void SaveGroup(Domain.DTO.Group group);
        void DeleteGroup(Guid groupGuid);
        void UnDeleteGroup(Guid groupGuid);
    }
}