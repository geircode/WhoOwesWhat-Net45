using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.Domain.DTO;
using WhoOwesWhat.Domain.Interfaces;

namespace WhoOwesWhat.Domain
{
    public class UserRepository : IUserRepository
    {
        private readonly IUserRepositoryLogic _userRepositoryLogic;
        private readonly IUserCredentialQuery _userCredentialQuery;
        private readonly IPersonQuery _personQuery;
        private readonly IPersonCommand _personCommand;
        private readonly IPersonRepositoryLogic _personRepositoryLogic;
        private readonly IUserCredentialCommand _credentialCommand;
        private readonly IHashUtils _hashUtils;

        public UserRepository(IUserRepositoryLogic userRepositoryLogic, 
            IUserCredentialQuery userCredentialQuery, 
            IPersonQuery personQuery,
            IPersonCommand personCommand, 
            IPersonRepositoryLogic personRepositoryLogic,
            IUserCredentialCommand credentialCommand,
            IHashUtils hashUtils)
        {
            _userRepositoryLogic = userRepositoryLogic;
            _userCredentialQuery = userCredentialQuery;
            _personQuery = personQuery;
            _personCommand = personCommand;
            _personRepositoryLogic = personRepositoryLogic;
            _credentialCommand = credentialCommand;
            _hashUtils = hashUtils;
        }

        public bool AuthenticateUser(string username, string password)
        {
            Guard.NotNullOrEmpty(() => username, username);
            Guard.NotNullOrEmpty(() => password, password);

            UserCredential userCredential = _userCredentialQuery.GetUserCredential(username);
            if (userCredential == null)
            {
                return false;
            }
            var passwordHash = _hashUtils.GetHashString(password);
            var isAuthenticated = userCredential.PasswordHash == passwordHash;
            return isAuthenticated;
        }

        /// <summary>
        /// Create new Online UserCredential. (Not Online or Offline Friend)
        /// </summary>
        public void CreateUser(Guid personGuid, string displayname, string username, string email, string mobile, string password)
        {
            Guard.NotNull(() => personGuid, personGuid);
            Guard.NotNullOrEmpty(() => displayname, displayname);
            Guard.NotNullOrEmpty(() => username, username);
            Guard.NotNullOrEmpty(() => email, email);
            Guard.NotNullOrEmpty(() => password, password);

            Guard.IsValid(() => personGuid, personGuid, ValidatePersonGuid, "PersonGuid can not be empty");
            Guard.IsValid(() => personGuid, personGuid, _userCredentialQuery.IsNotInUsePersonGuid, "UserCredential is already created for this personGuid");
            Guard.IsValid(() => email, email, _userCredentialQuery.IsUniqueEmail, "email was not unique");
            Guard.IsValid(() => username, username, _userCredentialQuery.IsUniqueUsername, "username was not unique");

            var person = _personQuery.GetPerson(personGuid);
            if (person == null)
            {
                person = _personRepositoryLogic.MapToPerson(personGuid, displayname, mobile);
                _personCommand.SavePerson(person);
                  
            }

            var passwordHash = _hashUtils.GetHashString(password);
            AddUserCredentialModel credential = _userRepositoryLogic.MapToAddUserCredentialModel(personGuid, username, email, passwordHash);

            _credentialCommand.AddUserCredential(credential);
        }

        private bool ValidatePersonGuid(Guid guid)
        {
            return guid != Guid.Empty;
        }

        public class UserRepositoryException : Exception
        {
            public UserRepositoryException(string message)
                : base(message)
            {
            }
        }
    }



}
