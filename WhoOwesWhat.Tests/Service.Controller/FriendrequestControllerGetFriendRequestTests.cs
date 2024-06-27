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

    [TestFixture]
    public class FriendrequestControllerGetFriendRequestTests
    {
        private static MoqMockingKernel _kernel;

        public FriendrequestControllerGetFriendRequestTests()
        {
            _kernel = new MoqMockingKernel();
            _kernel.Bind<IFriendrequestController>().To<FriendrequestController>();
        }

        [SetUp]
        public void SetUp()
        {
            _kernel.Reset();
        }

        [Test]
        public void When_getting_pending_friendrequests_for_a_person__it_should_succeed()
        {

            var controller = _kernel.Get<IFriendrequestController>();
            var request = new GetFriendrequestsRequest()
            {
                username = "Jolly",
                password = "Good",
            };

            //setup the mock
            var userCredentialRepositoryMock = _kernel.GetMock<IUserRepository>();
            userCredentialRepositoryMock.Setup(mock => mock.AuthenticateUser(request.username, request.password)).Returns(true);

            var friendrequestRepositoryMock = _kernel.GetMock<IFriendrequestRepository>();
            var friendrequests = new List<UserCredential>();
            friendrequestRepositoryMock.Setup(mock => mock.GetFriendsrequestsToPerson(request.username)).Returns(friendrequests);

            var response = controller.GetFriendrequests(request);

            response.isSuccess.Should().BeTrue();

        }        

       

    }
}

