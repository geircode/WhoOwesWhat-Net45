using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhoOwesWhat.Domain.DTO;

namespace WhoOwesWhat.Domain.Interfaces
{
    public interface IPersonRepository
    {
    }

    public interface IPersonRepositoryLogic
    {
        Domain.DTO.Person MapToPerson(Guid personGuid, string displayname, string mobile);
    }

}
