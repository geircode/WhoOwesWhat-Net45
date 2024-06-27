using System;
using System.Collections.Generic;
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
    public class SyncControllerTest_SyncOfflineFriend
    {
        private static MoqMockingKernel _kernel;
        List<SyncFriendsRequest.Friend> _friends;
        SyncFriendsRequest.Friend olavOnline;

        public SyncControllerTest_SyncOfflineFriend()
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
                personGuid = new Guid("01cebb3d-4747-4747-4747-168c6f2b63b3"),
                displayname = "Marianne_Offline",
                isFriendDeleted = false
            });

            olavOnline = new SyncFriendsRequest.Friend()
            {
                personGuid = new Guid("01cebb3d-3333-3333-3333-168c6f2b63b2"),
                displayname = "Olav_Online",
                isFriendDeleted = false
            };
            _friends.Add(olavOnline);
        }

        //[Test]
        //public void When_synchronizing_friends_to_a_username__it_should_succeed()
        //{
        //    var request = new SyncFriendsRequest()
        //    {
        //        username = "smurf@smurf.com",
        //        password = "smurf",
        //        friends = _friends
        //    };


        //    //setup the mock
        //    var userCredentialRepositoryMock = _kernel.GetMock<IUserRepository>();
        //    userCredentialRepositoryMock.Setup(mock => mock.AuthenticateUser(request.username, request.password)).Returns(true);

        //    var friendRepositoryMock = _kernel.GetMock<IFriendRepository>();
        //    friendRepositoryMock.Setup(mock => mock.SyncOfflineFriend(request.username, It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()));//.Returns(true);

        //    var syncController = _kernel.Get<ISyncController>(); // this will inject the mocked IBar into our normal MyFoo implementation

        //    var response = syncController.SyncFriends(request);

        //    response.isSuccess.Should().Be(true);
        //    userCredentialRepositoryMock.VerifyAll();
        //}          
        
        //[Test]
        //public void When_synchronizing_online_friends_to_a_username__it_should_succeed()
        //{
        //    var request = new SyncFriendsRequest()
        //    {
        //        username = "smurf@smurf.com",
        //        password = "smurf",
        //        friends = _friends
        //    };


        //    //setup the mock
        //    var userCredentialRepositoryMock = _kernel.GetMock<IUserRepository>();
        //    userCredentialRepositoryMock.Setup(mock => mock.AuthenticateUser(request.username, request.password)).Returns(true);

        //    var userCredentialQueryMock = _kernel.GetMock<IUserCredentialQuery>();
        //    userCredentialQueryMock.Setup(mock => mock.IsOnlinePerson(olavOnline.personGuid)).Returns(true);

        //    var friendRepositoryMock = _kernel.GetMock<IFriendRepository>();
        //    friendRepositoryMock.Setup(mock => mock.SyncOfflineFriend(request.username, It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()));
        //    friendRepositoryMock.Setup(mock => mock.DeleteOrUndeleteOnlineFriend(request.username, olavOnline.personGuid, It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()));

        //    var syncController = _kernel.Get<ISyncController>(); // this will inject the mocked IBar into our normal MyFoo implementation

        //    var response = syncController.SyncFriends(request);

        //    response.isSuccess.Should().Be(true);
        //    userCredentialRepositoryMock.VerifyAll();
        //    userCredentialQueryMock.VerifyAll();
        //    friendRepositoryMock.VerifyAll();
        //}        
        
       
       

    }
}

