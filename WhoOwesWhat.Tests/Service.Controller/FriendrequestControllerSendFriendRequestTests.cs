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
    public class FriendrequestControllerSendFriendRequestTests
    {
        private static MoqMockingKernel _kernel;

        public FriendrequestControllerSendFriendRequestTests()
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
        public void When_sending_a_friendrequest__it_should_succeed()
        {

            var controller = _kernel.Get<IFriendrequestController>();
            SendFriendrequest request = new SendFriendrequest()
            {
                username = "Jolly",
                password = "Good",
                friendUsername = "smurf@smurf.com"
            };

            //setup the mock
            var userCredentialRepositoryMock = _kernel.GetMock<IUserRepository>();
            userCredentialRepositoryMock.Setup(mock => mock.AuthenticateUser(request.username, request.password)).Returns(true);

            var friendrequestRepositoryMock = _kernel.GetMock<IFriendrequestRepository>();
            friendrequestRepositoryMock.Setup(mock => mock.SendFriendrequest(request.username, request.friendUsername));

            var response = controller.SendFriendrequest(request);

            response.isSuccess.Should().BeTrue();

        }        

       

    }
}

