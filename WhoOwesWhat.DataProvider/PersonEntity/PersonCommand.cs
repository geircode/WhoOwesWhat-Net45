using System;
using System.Linq;
using log4net;
using WhoOwesWhat.DataProvider.Interfaces;

namespace WhoOwesWhat.DataProvider.PersonEntity
{
    public class PersonCommand : IPersonCommand
    {
        private readonly IWhoOwesWhatContext _whoOwesWhatContext;
        private ILog _log;
        private readonly IPersonDataProviderLogic _personDataProviderLogic;

        public PersonCommand(IWhoOwesWhatContext whoOwesWhatContext, ILog log, IPersonDataProviderLogic personDataProviderLogic)
        {
            _whoOwesWhatContext = whoOwesWhatContext;
            _log = log;
            _personDataProviderLogic = personDataProviderLogic;
        }

        public void  SavePerson(Domain.DTO.Person person)
        {
            var personDb = _whoOwesWhatContext.GetPersonSqlRepository().GetAll().SingleOrDefault(a => a.PersonGuid == person.PersonGuid);
            if (personDb == null)
            {
                personDb = new Entity.Person();
                personDb.PersonGuid = person.PersonGuid;
                _whoOwesWhatContext.GetPersonSqlRepository().Add(personDb);
            }

            personDb.Displayname = person.Displayname;
            personDb.Mobile = person.Mobile;
            personDb.IsDeleted = person.IsDeleted;
            
            _whoOwesWhatContext.SaveChanges();
        }

        public void DeletePerson(Guid personGuid)
        {
            var personDb = _whoOwesWhatContext.GetPersonSqlRepository().GetAll().Single(a => a.PersonGuid == personGuid);
            if (personDb == null)
            {
                throw new PersonCommandException("Unable to find the Person to delete.");
            }

            personDb.IsDeleted = true;

            
            _whoOwesWhatContext.SaveChanges();

            
        }

        public void UnDeletePerson(Guid personGuid)
        {
            var personDb = _whoOwesWhatContext.GetPersonSqlRepository().GetAll().Single(a => a.PersonGuid == personGuid);
            if (personDb == null)
            {
                throw new PersonCommandException("Unable to find the Person to undelete.");
            }

            personDb.IsDeleted = false;


            _whoOwesWhatContext.SaveChanges();
        }

        public class PersonCommandException : Exception
        {
            public PersonCommandException(string message)
                : base(message)
            {
            }
        }
    }


}
