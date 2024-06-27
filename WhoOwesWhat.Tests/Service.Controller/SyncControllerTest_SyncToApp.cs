using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using FluentAssertions;
using Moq;
using Ninject;
using Ninject.MockingKernel.Moq;
using NUnit.Framework;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.Domain;
using WhoOwesWhat.Domain.DTO;
using WhoOwesWhat.Domain.Interfaces;
using WhoOwesWhat.Service.Controller;
using WhoOwesWhat.Service.DTO;

namespace WhoOwesWhat.Tests.Service.Controller
{
    // ReSharper disable InconsistentNaming

    [TestFixture]
    public class SyncControllerTest_SyncToApp
    {
        private static MoqMockingKernel _kernel;
        List<SyncFriendsRequest.Friend> _friends;

        public SyncControllerTest_SyncToApp()
        {
            _kernel = new MoqMockingKernel();
            _kernel.Bind<ISyncController>().To<SyncController>();
        }

        [SetUp]
        public void SetUp()
        {
            _kernel.Reset();

            _friends = new List<SyncFriendsRequest.Friend>();
            _friends.Add(new SyncFriendsRequest.Friend()
            {
                personGuid = new Guid("01cebb3d-53fa-4768-9561-168c6f2b63b1"),
                displayname = "Beate_Offline",
                isFriendDeleted = false
            });            
            _friends.Add(new SyncFriendsRequest.Friend()
            {
                personGuid = new Guid("01cebb3d-53fa-4768-9561-168c6f2b63b2"),
                displayname = "Olav_Online",
                isFriendDeleted = false
            });            
            _friends.Add(new SyncFriendsRequest.Friend()
            {
                personGuid = new Guid("01cebb3d-53fa-4768-9561-168c6f2b63b3"),
                displayname = "Marianne_Offline",
                isFriendDeleted = false
            });            
        }

        [Test]
        public void When_synchronizing_friends_to_a_username__it_should_succeed()
        {
            SyncFriendsToAppRequest request = new SyncFriendsToAppRequest()
            {
                username = "smurf@smurf.com",
                password = "smurf",
            };


            //setup the mock
            var userCredentialRepositoryMock = _kernel.GetMock<IUserRepository>();
            userCredentialRepositoryMock.Setup(mock => mock.AuthenticateUser(request.username, request.password)).Returns(true);


            //var friendQueryMock = _kernel.GetMock<IFriendQuery>();
            //friendQueryMock.Setup(mock => mock.GetFriendsToApp(request.username)).Returns(true);//;

            var friendRepositoryMock = _kernel.GetMock<IFriendRepository>();
            List<GetFriendsToAppModel> friendsToAppModel = new List<GetFriendsToAppModel>();
            friendsToAppModel.Add(new GetFriendsToAppModel()
            {
                OwnerGuid = new Guid("95498E50-E551-4D2F-8494-0F93A3D752FB"),
                FriendGuid = new Guid("DDA26A7E-5EDD-4F98-AE00-778DC43807AC"),
                Displayname = "Geir",
                IsFriendDeleted = false,
                IsPersonDeleted = false,
                IsFriendOnlinePerson = true
            });            
            friendsToAppModel.Add(new GetFriendsToAppModel()
            {
                OwnerGuid = new Guid("5097EAA7-E7FE-42E2-990B-0C5290CCFB4D"),
                FriendGuid = new Guid("20C26F23-60A4-4CC0-A3E7-96C517D375B4"),
                Displayname = "Marianne",
                IsFriendDeleted = false,
                IsPersonDeleted = false,
                IsFriendOnlinePerson = false
            });
            friendRepositoryMock.Setup(mock => mock.GetFriendsToApp(request.username)).Returns(friendsToAppModel);

            var syncController = _kernel.Get<ISyncController>(); 

            SyncFriendsToAppResponse response = syncController.SyncFriendsToApp(request);

            response.isSuccess.Should().Be(true);
            var geir = response.friends.First(a => a.displayname.Equals("Geir"));

            geir.ownerGuid.Should().Be(friendsToAppModel[0].OwnerGuid);
            geir.friendGuid.Should().Be(friendsToAppModel[0].FriendGuid);
            geir.displayname.Should().Be(friendsToAppModel[0].Displayname);
            geir.isFriendDeleted.Should().Be(friendsToAppModel[0].IsFriendDeleted);
            geir.isPersonDeleted.Should().Be(friendsToAppModel[0].IsPersonDeleted);
            geir.isFriendOnlinePerson.Should().Be(friendsToAppModel[0].IsFriendOnlinePerson);

            userCredentialRepositoryMock.VerifyAll();
        }        
        
       
       

    }
}

