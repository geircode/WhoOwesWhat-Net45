using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhoOwesWhat.DataProvider.Interfaces
{
    public interface IPersonQuery
    {
        bool IsUniquePersonGuid(Guid personGuid);
        Domain.DTO.Person GetPerson(Guid personGuid);
        Domain.DTO.Person GetPersonByUsername(string username);
    }
}
