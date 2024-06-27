using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhoOwesWhat.Domain.DTO;

namespace WhoOwesWhat.Domain.Interfaces
{
    public interface IFriendrequestRepository
    {
        void SendFriendrequest(string username, string friendUsername);
        List<UserCredential> GetFriendsrequestsToPerson(string username);
    }


    public interface IAcceptFriendrequestLogic
    {
        void AcceptFriendrequest(string username, string friendUsername);
    }    

    
    

}
