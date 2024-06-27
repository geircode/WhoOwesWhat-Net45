using System;
using WhoOwesWhat.Domain.DTO;

namespace WhoOwesWhat.DataProvider.Interfaces
{
    public interface IPersonCommand
    {
        void SavePerson(Person person);
        void DeletePerson(Guid personGuid);
        void UnDeletePerson(Guid personGuid);
    }
}