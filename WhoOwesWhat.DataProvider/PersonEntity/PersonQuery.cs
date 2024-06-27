using System;
using System.Linq;
using log4net;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.Domain.DTO;
using UserCredential = WhoOwesWhat.DataProvider.Entity.UserCredential;

namespace WhoOwesWhat.DataProvider.PersonEntity
{
    public class PersonQuery : IPersonQuery
    {
        private readonly IWhoOwesWhatContext _whoOwesWhatContext;
        private ILog _log;
        private readonly IPersonDataProviderLogic _personDataProviderLogic;

        public PersonQuery(IWhoOwesWhatContext whoOwesWhatContext, ILog log, IPersonDataProviderLogic personDataProviderLogic)
        {
            _whoOwesWhatContext = whoOwesWhatContext;
            _log = log;
            _personDataProviderLogic = personDataProviderLogic;
        }

        public bool IsUniquePersonGuid(Guid personGuid)
        {
            return _whoOwesWhatContext.GetPersonSqlRepository().GetAll().FirstOrDefault(a => a.PersonGuid == personGuid) == null;
        }

        public Domain.DTO.Person GetPerson(Guid personGuid)
        {
            var personDb = _whoOwesWhatContext.GetPersonSqlRepository().GetAll().SingleOrDefault(a => a.PersonGuid == personGuid);
            return personDb == null ? null : _personDataProviderLogic.MapToDomain(personDb);
        }

        public Person GetPersonByUsername(string username)
        {
            UserCredential credential = _whoOwesWhatContext.GetUserCredentialSqlRepository().GetAll().SingleOrDefault(a => a.Username == username);
            if (credential == null)
            {
                return null;
            }
            var personDb = _whoOwesWhatContext.GetPersonSqlRepository().GetAll().SingleOrDefault(a => a.PersonId == credential.PersonId);
            return personDb == null ? null : _personDataProviderLogic.MapToDomain(personDb);
        }
    }
}
