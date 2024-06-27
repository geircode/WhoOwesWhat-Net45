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
    public class SyncControllerTest_SyncPost
    {
        private static MoqMockingKernel _kernel;

        public SyncControllerTest_SyncPost()
        {
            _kernel = new MoqMockingKernel();
            _kernel.Bind<ISyncController>().To<SyncController>();
            _kernel.Bind<ITestPostQuery>().To<TestPostQuery>();
        }

        [SetUp]
        public void SetUp()
        {
            _kernel.Reset();
        }

        [Test]
        public void When_synchronizing_posts_to_a_username__it_should_succeed()
        {
            var postTestQuery = _kernel.Get<ITestPostQuery>(); 

            var request = new SyncPostsRequest()
            {
                username = "smurf@smurf.com",
                password = "smurf",
                lastSynchronizedToServer = new DateTime(2015, 04, 02),

                posts = postTestQuery.GetSyncPostsRequestPost_Hooters()
            };


            //setup the mock
            var userCredentialRepositoryMock = _kernel.GetMock<IUserRepository>();
            userCredentialRepositoryMock.Setup(mock => mock.AuthenticateUser(request.username, request.password)).Returns(true);

            var postRepositoryMock = _kernel.GetMock<ISyncPostsCommand>();
            postRepositoryMock.Setup(mock => mock.SyncPosts(It.IsAny<string>(), It.IsAny<List<SyncPostModel>>()));

            var syncController = _kernel.Get<ISyncController>(); 

            var response = syncController.SyncPosts(request);

            response.isSuccess.Should().Be(true);
            userCredentialRepositoryMock.VerifyAll();
            postRepositoryMock.VerifyAll();
        }


    }
}

