using System;
using System.Collections.Generic;
using System.Linq;
using WhoOwesWhat.DataProvider.Entity;

namespace WhoOwesWhat.DataProvider.UserCredentialEntity
{
    public interface IUserCredentialContext
    {
        List<Entity.UserCredential> GetUserCredentials();
        Entity.UserCredential GetUserCredential(string username);
        UserCredential GetUserCredentialByPersonGuid(Guid personGuid);
    }

    public class UserCredentialContext : IUserCredentialContext
    {
        private readonly IWhoOwesWhatContext _whoOwesWhatContext;

        public UserCredentialContext(IWhoOwesWhatContext whoOwesWhatContext)
        {
            _whoOwesWhatContext = whoOwesWhatContext;
        }

        public List<Entity.UserCredential> GetUserCredentials()
        {
            var result = _whoOwesWhatContext.GetUserCredentialSqlRepository().GetAllAsList();
            return result;            
        }

        public Entity.UserCredential GetUserCredential(string username)
        {
            var result =
                _whoOwesWhatContext.GetUserCredentialSqlRepository().GetAll().SingleOrDefault((a => a.Username == username));
            return result;

        }

        public UserCredential GetUserCredentialByPersonGuid(Guid personGuid)
        {
            var result = _whoOwesWhatContext.GetUserCredentialSqlRepository().GetAll().SingleOrDefault((a => a.Person.PersonGuid == personGuid));
            return result;
        }
    }


}
