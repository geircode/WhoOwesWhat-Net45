using WhoOwesWhat.Domain.DTO;

namespace WhoOwesWhat.DataProvider.Interfaces
{
    public interface IUserCredentialCommand
    {
        void AddUserCredential(AddUserCredentialModel userCredential);
    }
}