using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.Domain.DTO;
using WhoOwesWhat.Domain.Interfaces;
using WhoOwesWhat.Service.DTO;

namespace WhoOwesWhat.Service.Controller
{
    public interface ISyncController
    {
        BasicResponse SyncFriends(SyncFriendsRequest request);
        SyncFriendsToAppResponse SyncFriendsToApp(SyncFriendsToAppRequest request);
        SyncPostsResponse SyncPosts(SyncPostsRequest request);
        SyncGroupsResponse SyncGroups(SyncGroupsRequest request);
    }

    public class SyncController : ISyncController
    {
        private readonly IUserRepository _userRepository;
        private readonly IFriendRepository _friendRepository;
        private readonly IUserCredentialQuery _userCredentialQuery;
        private readonly ISyncPostQuery _syncPostQuery;
        private readonly ISyncPostsCommand _syncPostsCommand;
        private readonly ISyncControllerLogic _syncControllerLogic;
        private readonly ISyncGroupsCommand _syncGroupsCommand;
        private readonly ISyncGroupQuery _syncGroupQuery;
        private readonly IDeleteFriendCommand _deleteFriendCommand;

        public SyncController(
            IUserRepository userRepository
            , IFriendRepository friendRepository
            , IUserCredentialQuery userCredentialQuery
            , ISyncPostQuery syncPostQuery
            , ISyncPostsCommand syncPostsCommand
            , ISyncControllerLogic syncControllerLogic
            , ISyncGroupsCommand syncGroupsCommand
            , ISyncGroupQuery syncGroupQuery
            , IDeleteFriendCommand deleteFriendCommand
            )
        {
            _userRepository = userRepository;
            _friendRepository = friendRepository;
            _userCredentialQuery = userCredentialQuery;
            _syncPostQuery = syncPostQuery;
            _syncPostsCommand = syncPostsCommand;
            _syncControllerLogic = syncControllerLogic;
            _syncGroupsCommand = syncGroupsCommand;
            _syncGroupQuery = syncGroupQuery;
            _deleteFriendCommand = deleteFriendCommand;
        }

        /// <summary>
        /// Både Online Og Offline friends kan oppdateres via denne funksjonen, men OnlinePersons kan kun bli slettet
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BasicResponse SyncFriends(SyncFriendsRequest request)
        {
            Stopwatch before = Stopwatch.StartNew();

            var response = new BasicResponse();

            var isAuthenticated = _userRepository.AuthenticateUser(request.username, request.password);
            if (isAuthenticated)
            {
                foreach (var friend in request.friends)
                {
                    if (_userCredentialQuery.IsOnlinePerson(friend.personGuid))
                    {
                        if (friend.isUsedInPosts)
                        {
                            _deleteFriendCommand.UndeleteOnlineFriend(request.username, friend.personGuid);
                        }

                        if (friend.isFriendDeleted)
                        {
                            _deleteFriendCommand.DeleteOnlineFriend(request.username, friend.personGuid);
                        }
                    }
                    else
                    {
                        _friendRepository.SaveOfflineFriend(request.username, friend.personGuid, friend.displayname);

                        if (friend.isUsedInPosts)
                        {
                            _deleteFriendCommand.UndeleteOfflineFriend(request.username, friend.personGuid);
                        }

                        if (friend.isFriendDeleted)
                        {
                            _deleteFriendCommand.DeleteOfflineFriend(request.username, friend.personGuid);
                        }
                    }
                }
            }

            before.Stop();
            response.executionTimeInMilliseconds = before.ElapsedMilliseconds;

            // No exceptions where thrown, so it was a success.
            response.isSuccess = true;
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public SyncFriendsToAppResponse SyncFriendsToApp(SyncFriendsToAppRequest request)
        {
            Stopwatch before = Stopwatch.StartNew();
            var response = new SyncFriendsToAppResponse();

            var isAuthenticated = _userRepository.AuthenticateUser(request.username, request.password);
            if (isAuthenticated)
            {
                var friends = _friendRepository.GetFriendsToApp(request.username);
                foreach (var friend in friends)
                {
                    response.friends.Add(new SyncFriendsToAppResponse.Friend()
                    {
                        friendGuid = friend.FriendGuid,
                        ownerGuid = friend.OwnerGuid,
                        displayname = friend.Displayname,
                        isFriendDeleted = friend.IsFriendDeleted,
                        isFriendOnlinePerson = friend.IsFriendOnlinePerson,
                        isPersonDeleted = friend.IsPersonDeleted,
                    });
                }
            }
            before.Stop();
            response.executionTimeInMilliseconds = before.ElapsedMilliseconds;
            // No exceptions where thrown, so it was a success.
            response.isSuccess = true;
            return response;
        }

        public SyncPostsResponse SyncPosts(SyncPostsRequest request)
        {
            Stopwatch before = Stopwatch.StartNew();

            var response = new SyncPostsResponse();
            var isAuthenticated = _userRepository.AuthenticateUser(request.username, request.password);
            if (isAuthenticated)
            {
                List<SyncPostModel> syncPostModels = _syncControllerLogic.MapSyncPostsRequestToSyncPostModel(request.posts);

                List<Post> postConflicts = _syncPostQuery.GetPostConflicts(request.username, syncPostModels);

                _syncPostsCommand.SyncPosts(request.username, syncPostModels);
                
                // Get unsynchronized Posts including Posts that have been updated, but without PostConflicts
                List<Post> unsyncPosts = _syncPostQuery.GetUnsynchronizedPostsWithoutPostConflicts(request.username, request.lastSynchronizedToServer, postConflicts);

                response.postConflicts = _syncControllerLogic.MapPostToSyncPostsResponsePost(postConflicts);
                //// Get unsynchronized Posts including Posts that have been updated
                response.unsyncPosts = _syncControllerLogic.MapPostToSyncPostsResponsePost(unsyncPosts);

                // Send nye Poster og Poster som er i konflikt til App

                // No exceptions where thrown, so it was a success.

                response.isSuccess = true;
            }

            before.Stop();
            response.executionTimeInMilliseconds = before.ElapsedMilliseconds;
          
            return response;
        }

        public SyncGroupsResponse SyncGroups(SyncGroupsRequest request)
        {
            Stopwatch before = Stopwatch.StartNew();
            var response = new SyncGroupsResponse();

            var isAuthenticated = _userRepository.AuthenticateUser(request.username, request.password);
            if (isAuthenticated)
            {
                List<SyncGroupModel> syncGroupModels = _syncControllerLogic.MapSyncGroupsRequestToSyncGroupModel(request.groups);

                _syncGroupsCommand.SyncGroups(request.username, syncGroupModels);

                // Get unsynchronized Groups including Groups that have been updated
                List<Group> unsyncGroups = _syncGroupQuery.GetUnsynchronizedGroups(request.username, request.lastSynchronizedToServer);

                //// Get unsynchronized Groups including Groups that have been updated
                response.groups = _syncControllerLogic.MapGroupToSyncGroupsResponseGroup(unsyncGroups);
                // No exceptions where thrown, so it was a success.
                response.isSuccess = true;
            }

            before.Stop();
            response.executionTimeInMilliseconds = before.ElapsedMilliseconds;

            return response;
        }

    }

}