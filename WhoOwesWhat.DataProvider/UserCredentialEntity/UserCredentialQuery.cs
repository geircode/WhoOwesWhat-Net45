using System;
using System.Collections.Generic;
using System.Linq;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.DataProvider.PersonEntity;
using WhoOwesWhat.Domain.DTO;

namespace WhoOwesWhat.DataProvider.UserCredentialEntity
{
    public class UserCredentialQuery : IUserCredentialQuery
    {
        private readonly IWhoOwesWhatContext _whoOwesWhatContext;
        private readonly IUserCredentialDataProviderLogic _userCredentialDataProviderLogic;
        private readonly IUserCredentialContext _userCredentialContext;
        private readonly IPersonContext _personContext;

        public UserCredentialQuery(IWhoOwesWhatContext whoOwesWhatContext, IUserCredentialDataProviderLogic userCredentialDataProviderLogic, IUserCredentialContext userCredentialContext, IPersonContext personContext)
        {
            _whoOwesWhatContext = whoOwesWhatContext;
            _userCredentialDataProviderLogic = userCredentialDataProviderLogic;
            _userCredentialContext = userCredentialContext;
            _personContext = personContext;
        }

        public List<UserCredential> GetUserCredentials()
        {
            var result = _whoOwesWhatContext.GetUserCredentialSqlRepository().GetAll().Select(a => _userCredentialDataProviderLogic.MapToDomain(a)).ToList();
            return result;
            
        }

        public UserCredential GetUserCredential(string username)
        {
            var userCredential = _userCredentialContext.GetUserCredential(username);
            if (userCredential == null)
            {
                return null;
            }

            userCredential.Person = _personContext.GetPerson(userCredential.PersonId);
            return _userCredentialDataProviderLogic.MapToDomain(userCredential);
        }

        public UserCredential GetUserCredentialByPersonGuid(Guid personGuid)
        {
            var userCredential = _userCredentialContext.GetUserCredentialByPersonGuid(personGuid);
            if (userCredential == null)
            {
                return null;
            }

            userCredential.Person = _personContext.GetPerson(userCredential.PersonId);
            return _userCredentialDataProviderLogic.MapToDomain(userCredential);
        }

        public bool IsUniqueEmail(string email)
        {
            return _whoOwesWhatContext.GetUserCredentialSqlRepository().GetAll().FirstOrDefault(a => a.Email == email) == null;
        }

        public bool IsUniqueUsername(string username)
        {
            return _whoOwesWhatContext.GetUserCredentialSqlRepository().GetAll().FirstOrDefault(a => a.Username == username) == null;
        }

        public bool IsNotInUsePersonGuid(Guid personGuid)
        {
            return _whoOwesWhatContext.GetUserCredentialSqlRepository().GetAll().All(a => a.Person.PersonGuid != personGuid);
        }

        public bool IsNotOnlinePerson(Guid personGuid)
        {
            return _whoOwesWhatContext.GetUserCredentialSqlRepository().GetAll().FirstOrDefault(a => a.Person.PersonGuid == personGuid) == null;
        }        
        
        public bool IsOnlinePerson(Guid personGuid)
        {
            return _whoOwesWhatContext.GetUserCredentialSqlRepository().GetAll().FirstOrDefault(a => a.Person.PersonGuid == personGuid) != null;
        }
    }

    public class UserCredentialDataProviderException : Exception
    {
        public UserCredentialDataProviderException(string message)
            : base(message)
        {
        }
    }
}
