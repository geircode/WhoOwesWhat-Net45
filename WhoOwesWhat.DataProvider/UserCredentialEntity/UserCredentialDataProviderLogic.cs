using WhoOwesWhat.DataProvider.PersonEntity;
using WhoOwesWhat.Domain.DTO;

namespace WhoOwesWhat.DataProvider.UserCredentialEntity
{
    public interface IUserCredentialDataProviderLogic
    {
        UserCredential MapToDomain(Entity.UserCredential source);
    }

    public class UserCredentialDataProviderLogic : IUserCredentialDataProviderLogic
    {
        private readonly IPersonDataProviderLogic _personDataProviderLogic;

        public UserCredentialDataProviderLogic(IPersonDataProviderLogic personDataProviderLogic)
        {
            _personDataProviderLogic = personDataProviderLogic;
        }

        public UserCredential MapToDomain(Entity.UserCredential source)
        {
            var targetDomain = new Domain.DTO.UserCredential();
            targetDomain.Email = source.Email;
            targetDomain.Username = source.Username;
            targetDomain.PasswordHash = source.PasswordHash;
            targetDomain.Person = _personDataProviderLogic.MapToDomain(source.Person);
            return targetDomain;
        }
    }
}
