using System;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.Domain.DTO;
using WhoOwesWhat.Domain.Interfaces;

namespace WhoOwesWhat.Domain
{
    public class PersonRepositoryLogic : IPersonRepositoryLogic
    {
        public Person MapToPerson(Guid personGuid, string displayname, string mobile)
        {
            var user = new Domain.DTO.Person()
            {
                PersonGuid = personGuid,
                Displayname = displayname,
                Mobile = mobile,
            };

            return user;
        }
    }
}
