using System.Linq;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.Domain.DTO;

namespace WhoOwesWhat.DataProvider.UserCredentialEntity
{
    public class UserCredentialCommand : IUserCredentialCommand
    {
        private IWhoOwesWhatContext _whoOwesWhatContext;
        private IUserCredentialDataProviderLogic _userCredentialDataProviderLogic;

        public UserCredentialCommand(IWhoOwesWhatContext whoOwesWhatContext, IUserCredentialDataProviderLogic userCredentialDataProviderLogic)
        {
            _whoOwesWhatContext = whoOwesWhatContext;
            _userCredentialDataProviderLogic = userCredentialDataProviderLogic;
        }

        public void AddUserCredential(AddUserCredentialModel userCredential)
        {
            var credentialDb = _whoOwesWhatContext.GetUserCredentialSqlRepository().GetAll().SingleOrDefault(a => a.Username == userCredential.Username);
            if (credentialDb == null)
            {
                credentialDb = new Entity.UserCredential();

                var personDb = _whoOwesWhatContext.GetPersonSqlRepository().GetAll().SingleOrDefault(a => a.PersonGuid == userCredential.PersonGuid);
                if (personDb == null)
                {
                    throw new UserCredentialDataProviderException("PersonGuid not found. Can not add UserCredential.");
                }
                credentialDb.Person = personDb;

                _whoOwesWhatContext.GetUserCredentialSqlRepository().Add(credentialDb);
            }

            credentialDb.Email = userCredential.Email;
            credentialDb.Username = userCredential.Username;
            credentialDb.PasswordHash = userCredential.PasswordHash;

            
            _whoOwesWhatContext.SaveChanges();

            
        }
    }

}
