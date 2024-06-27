using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Ninject;
using Ninject.MockingKernel.Moq;
using NUnit.Framework;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.Domain;
using WhoOwesWhat.Domain.DTO;
using WhoOwesWhat.Domain.Interfaces;
using WhoOwesWhat.Domain.Post;

namespace WhoOwesWhat.Tests
{

    [TestFixture]
    public class SyncPostsTests
    {
        private static MoqMockingKernel _kernel;

        public SyncPostsTests()
        {
            _kernel = new MoqMockingKernel();
            _kernel.Bind<ISyncPostCommand>().To<SyncPostCommand>();
            _kernel.Bind<ITestPostQuery>().To<TestPostQuery>();
        }

        [SetUp]
        public void SetUp()
        {
            _kernel.Reset();
        }

        [Test]
        public void When_synchronizing_a_new_Post__it_should_check_if_username_is_authorized_and_succeed()
        {
            var username = TestUserCredentialQuery.GeirUserCredential.Username;
            //setup the mock
            var userCredentialRepositoryMock = _kernel.GetMock<IUserCredentialQuery>();
            userCredentialRepositoryMock.Setup(mock => mock.GetUserCredential(username)).Returns(TestUserCredentialQuery.GeirUserCredential);

            var postQueryMock = _kernel.GetMock<IPostQuery>();
            postQueryMock.Setup(mock => mock.GetPostByPostGuid(It.IsAny<Guid>())).Returns((Post)null);

            var postCommandMock = _kernel.GetMock<IPostCommand>();
            postCommandMock.Setup(mock => mock.SavePost(It.IsAny<Post>()));

            var postTestQuery = _kernel.Get<ITestPostQuery>();
            var postRepository = _kernel.Get<ISyncPostCommand>(); // this will inject the mocked IBar into our normal MyFoo implementation

            var postHooters = postTestQuery.GetSyncPostModel_Hooters();
            postRepository.SyncPost(username, postHooters);

            userCredentialRepositoryMock.VerifyAll();
            postQueryMock.VerifyAll();
            postCommandMock.VerifyAll();
        }

        [Test]
        public void When_synchronizing_a_new_Post__it_should_check_if_username_is_authorized_and_fail()
        {
            var username = TestUserCredentialQuery.GeirUserCredential.Username;
            //setup the mock
            var userCredentialRepositoryMock = _kernel.GetMock<IUserCredentialQuery>();
            userCredentialRepositoryMock.Setup(mock => mock.GetUserCredential(username)).Returns(TestUserCredentialQuery.GeirUserCredential);

            var postQueryMock = _kernel.GetMock<IPostQuery>();
            postQueryMock.Setup(mock => mock.GetPostByPostGuid(It.IsAny<Guid>())).Returns((Post)null);

            var postCommandMock = _kernel.GetMock<IPostCommand>();
            postCommandMock.Setup(mock => mock.SavePost(It.IsAny<Post>()));

            var postTestQuery = _kernel.Get<ITestPostQuery>();
            var postRepository = _kernel.Get<ISyncPostCommand>(); // this will inject the mocked IBar into our normal MyFoo implementation

            var postHooters = postTestQuery.GetSyncPostModel_Hooters();
            Action act = () => postRepository.SyncPost(username, postHooters);
            act.ShouldThrow<SyncPostCommand.SyncPostCommandException>();


            userCredentialRepositoryMock.VerifyAll();
            postQueryMock.Verify(a => a.GetPostByPostGuid(It.IsAny<Guid>()), Times.Never);
            postCommandMock.Verify(a => a.SavePost(It.IsAny<Post>()), Times.Never);
        }

        [Test]
        public void When_synchronizing_an_existing_Post__it_should_check_if_the_dirtypost_has_a_conflicting_version_and_succeed()
        {
            var postTestQuery = _kernel.Get<ITestPostQuery>();

            var username = TestUserCredentialQuery.GeirUserCredential.Username;
            //setup the mock
            var userCredentialRepositoryMock = _kernel.GetMock<IUserCredentialQuery>();
            userCredentialRepositoryMock.Setup(mock => mock.GetUserCredential(username)).Returns(TestUserCredentialQuery.GeirUserCredential);

            var postQueryMock = _kernel.GetMock<IPostQuery>();
            var domainHooters = postTestQuery.GetDomainPost_Hooters();
            domainHooters.Version = 5;
            postQueryMock.Setup(mock => mock.GetPostByPostGuid(It.IsAny<Guid>())).Returns(domainHooters);

            var postCommandMock = _kernel.GetMock<IPostCommand>();
            postCommandMock.Setup(mock => mock.SavePost(It.IsAny<Post>()));

            var syncPostCommand = _kernel.Get<ISyncPostCommand>(); // ***

            var postHooters = postTestQuery.GetSyncPostModel_Hooters();
            postHooters.Version = 5;
            
            syncPostCommand.SyncPost(username, postHooters);

            domainHooters.Version.Should().Be(6);
            userCredentialRepositoryMock.VerifyAll();
            postQueryMock.Verify(a => a.GetPostByPostGuid(It.IsAny<Guid>()), Times.Once);
            postCommandMock.Verify(a => a.SavePost(It.IsAny<Post>()), Times.Once);
        }

        [Test]
        public void When_synchronizing_a_new_Post_with_Group__it_should_succeed() //existence of Group is checked in DataProvider
        {
            var postTestQuery = _kernel.Get<ITestPostQuery>();

            var username = TestUserCredentialQuery.GeirUserCredential.Username;
            //setup the mock
            var userCredentialRepositoryMock = _kernel.GetMock<IUserCredentialQuery>();
            userCredentialRepositoryMock.Setup(mock => mock.GetUserCredential(username)).Returns(TestUserCredentialQuery.GeirUserCredential);

            var postQueryMock = _kernel.GetMock<IPostQuery>();
            postQueryMock.Setup(mock => mock.GetPostByPostGuid(It.IsAny<Guid>())).Returns((Post) null);

            var postCommandMock = _kernel.GetMock<IPostCommand>();
            postCommandMock.Setup(mock => mock.SavePost(It.IsAny<Post>()));

            var syncPostCommand = _kernel.Get<ISyncPostCommand>(); // ***

            var postHooters = postTestQuery.GetSyncPostModel_Hooters();
            postHooters.GroupGuid = TestGroupQuery.HawaiiGroupsDomain.GroupGuid;
            syncPostCommand.SyncPost(username, postHooters);

            userCredentialRepositoryMock.VerifyAll();
            postQueryMock.Verify(a => a.GetPostByPostGuid(It.IsAny<Guid>()), Times.Once);
            postCommandMock.Verify(a => a.SavePost(It.IsAny<Post>()), Times.Once);
        }

    }
}

