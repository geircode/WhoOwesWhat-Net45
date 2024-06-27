using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhoOwesWhat.Domain.DTO;

namespace WhoOwesWhat.DataProvider.Interfaces
{
    public interface IUserCredentialQuery
    {
        List<UserCredential> GetUserCredentials();
        UserCredential GetUserCredential(string username);
        UserCredential GetUserCredentialByPersonGuid(Guid personGuid);

        bool IsUniqueEmail(string email);
        bool IsUniqueUsername(string username);
        bool IsNotInUsePersonGuid(Guid personGuid);
        bool IsNotOnlinePerson(Guid personGuid);
        bool IsOnlinePerson(Guid personGuid);
    }
}
