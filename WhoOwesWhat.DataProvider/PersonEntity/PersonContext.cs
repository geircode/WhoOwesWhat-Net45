using System;
using System.Linq;
using WhoOwesWhat.DataProvider.Entity;

namespace WhoOwesWhat.DataProvider.PersonEntity
{
    public interface IPersonContext
    {
        Entity.Person GetPerson(int personId);
        Person GetPersonByPersonGuid(Guid personGuid);
    }

    public class PersonContext : IPersonContext
    {
        private readonly IWhoOwesWhatContext _whoOwesWhatContext;

        public PersonContext(IWhoOwesWhatContext whoOwesWhatContext)
        {
            _whoOwesWhatContext = whoOwesWhatContext;
        }

        public Person GetPerson(int personId)
        {
            return _whoOwesWhatContext.GetPersonSqlRepository().GetAll().SingleOrDefault(a => a.PersonId == personId);
        }

        public Person GetPersonByPersonGuid(Guid personGuid)
        {
            return _whoOwesWhatContext.GetPersonSqlRepository().GetAll().SingleOrDefault(a => a.PersonGuid == personGuid);
        }
    }
}
