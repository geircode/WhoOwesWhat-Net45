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
    public class SyncPostsQueryTests
    {
        private static MoqMockingKernel _kernel;
        private ITestPostQuery _postTestQuery;

        public SyncPostsQueryTests()
        {
            _kernel = new MoqMockingKernel();
            _kernel.Bind<ISyncPostQuery>().To<SyncPostQuery>();
            _kernel.Bind<ITestPostQuery>().To<TestPostQuery>();
        }

        [SetUp]
        public void SetUp()
        {
            _kernel.Reset();

            _postTestQuery = _kernel.Get<ITestPostQuery>();
        }

        [Test]
        public void When_getting_unsynchronized_Posts__it_should_return_Posts_Updated_and_new()
        {
            List<Post> postsUpdated = new List<Post>();
            postsUpdated.Add(_postTestQuery.GetDomainPost_Hooters());

            var postQueryMock = _kernel.GetMock<IPostQuery>();
            postQueryMock.Setup(mock => mock.GetPostsCreatedByAndUpdatedAfter(It.IsAny<Guid>(), It.IsAny<DateTime>())).Returns(postsUpdated);

            var username = TestUserCredentialQuery.GeirUserCredential.Username;
            //setup the mock
            var userCredentialRepositoryMock = _kernel.GetMock<IUserCredentialQuery>();
            userCredentialRepositoryMock.Setup(mock => mock.GetUserCredential(username)).Returns(TestUserCredentialQuery.GeirUserCredential);

            var syncPostQuery = _kernel.Get<ISyncPostQuery>(); // this will inject the mocked IBar into our normal MyFoo implementation

            syncPostQuery.GetUnsynchronizedPosts(username, new DateTime(2010, 03, 29));

            userCredentialRepositoryMock.VerifyAll();
            postQueryMock.VerifyAll();
        }

    }
}

