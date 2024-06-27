using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhoOwesWhat.Domain.DTO;

namespace WhoOwesWhat.Domain.Interfaces
{
    public interface IUserRepository
    {
        bool AuthenticateUser(string username, string password);
        void CreateUser(Guid personGuid, string displayname, string username, string email, string mobile, string password);
    }    
    
    public interface IUserRepositoryLogic
    {
        AddUserCredentialModel MapToAddUserCredentialModel(Guid personGuid, string username, string email, string passwordHash);
    }

}
