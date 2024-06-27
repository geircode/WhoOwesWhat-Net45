using System.Collections.Generic;
using WhoOwesWhat.DataProvider.Debug;
using WhoOwesWhat.Service.Controller;
using WhoOwesWhat.Service.DTO;

namespace WhoOwesWhat.Service
{
    public partial class WhoOwesWhatService : ServiceStack.ServiceInterface.Service, IWhoOwesWhatService
    {
        private readonly IUserController _userController;
        private readonly IDataProviderDebug _dataProviderDebug;
        private readonly ISyncController _syncController;
        private readonly ErrorController _errorController;
        private readonly IFriendrequestController _friendrequestController;

        public WhoOwesWhatService(
            IUserController userController
            , IDataProviderDebug dataProviderDebug
            , ISyncController syncController
            , ErrorController errorController
            , IFriendrequestController friendrequestController
            )
        {
            _userController = userController;
            _dataProviderDebug = dataProviderDebug;
            _syncController = syncController;
            _errorController = errorController;
            _friendrequestController = friendrequestController;
        }

        public AuthenticateUserResponse Any(AuthenticateUserRequest request)
        {
            return _userController.AuthenticateUser(request);
        }

        public BasicResponse Any(CreateUserRequest request)
        {
            return _userController.CreateUser(request);
        }

        public GetPersonByEmailResponse Any(GetPersonByEmailRequest request)
        {
            return _userController.GetPersonByEmail(request);
        }

        public BasicResponse Any(SendFriendrequest request)
        {
            return _friendrequestController.SendFriendrequest(request);
        }

        public GetFriendrequestsReponse Any(GetFriendrequestsRequest request)
        {
            return _friendrequestController.GetFriendrequests(request);
        }


        public BasicResponse Any(AcceptFriendrequestRequest request)
        {
            return _friendrequestController.AcceptFriendrequest(request);
        }

        public BasicResponse Any(SyncFriendsRequest request)
        {
            return _syncController.SyncFriends(request);
        }

        public SyncFriendsToAppResponse Any(SyncFriendsToAppRequest request)
        {
            return _syncController.SyncFriendsToApp(request);
        }

        public SyncPostsResponse Any(SyncPostsRequest request)
        {
            return _syncController.SyncPosts(request);
        }

        public SyncGroupsResponse Any(SyncGroupsRequest request)
        {
            return _syncController.SyncGroups(request);
        }

        public BasicResponse Any(SaveErrorRequest request)
        {
            return _errorController.SaveError(request);
        }


    }
}