using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.Domain.DTO;
using WhoOwesWhat.Domain.Interfaces;
using WhoOwesWhat.Service.DTO;

namespace WhoOwesWhat.Service.Controller
{
    public interface IFriendrequestController
    {
        BasicResponse SendFriendrequest(SendFriendrequest request);
        GetFriendrequestsReponse GetFriendrequests(GetFriendrequestsRequest request);
        BasicResponse AcceptFriendrequest(AcceptFriendrequestRequest request);
    }

    public class FriendrequestController : IFriendrequestController
    {
        private readonly IUserRepository _userRepository;
        private readonly IFriendrequestRepository _friendrequestRepository;
        private readonly IAcceptFriendrequestLogic _acceptFriendrequestLogic;

        public FriendrequestController(IUserRepository userRepository, IFriendrequestRepository friendrequestRepository, IAcceptFriendrequestLogic acceptFriendrequestLogic)
        {
            _userRepository = userRepository;
            _friendrequestRepository = friendrequestRepository;
            _acceptFriendrequestLogic = acceptFriendrequestLogic;
        }

        public BasicResponse SendFriendrequest(SendFriendrequest request)
        {
            var response = new BasicResponse();

            var isAuthenticated = _userRepository.AuthenticateUser(request.username, request.password);
            if (isAuthenticated)
            {
                _friendrequestRepository.SendFriendrequest(request.username, request.friendUsername);
                response.isSuccess = true;
            }

            return response;
        }

        public GetFriendrequestsReponse GetFriendrequests(GetFriendrequestsRequest request)
        {
            var response = new GetFriendrequestsReponse();

            var isAuthenticated = _userRepository.AuthenticateUser(request.username, request.password);
            if (isAuthenticated)
            {
                List<UserCredential> friendrequests = _friendrequestRepository.GetFriendsrequestsToPerson(request.username);
                foreach (var friendrequest in friendrequests)
                {
                    response.friendrequests.Add(new GetFriendrequestsReponse.Friendrequest()
                    {
                        displayname = friendrequest.Person.Displayname,
                        username = friendrequest.Username,
                    });
                }
                response.isSuccess = true;
            }

            return response;
        }

        public BasicResponse AcceptFriendrequest(AcceptFriendrequestRequest request)
        {
            var response = new BasicResponse();

            var isAuthenticated = _userRepository.AuthenticateUser(request.username, request.password);
            if (isAuthenticated)
            {
                _acceptFriendrequestLogic.AcceptFriendrequest(request.username, request.friendUsername);
                response.isSuccess = true;
            }

            return response;
        }
    }
    
}