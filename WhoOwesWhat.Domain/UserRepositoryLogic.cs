using System;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.Domain.DTO;
using WhoOwesWhat.Domain.Interfaces;

namespace WhoOwesWhat.Domain
{
    public class UserRepositoryLogic : IUserRepositoryLogic
    {

        public AddUserCredentialModel MapToAddUserCredentialModel(Guid personGuid, string username, string email, string passwordHash)
        {
            var credential = new AddUserCredentialModel()
            {
                Email = email,
                Username = username,
                PasswordHash = passwordHash,
                PersonGuid = personGuid
            };

            return credential;
        }

    }
}
