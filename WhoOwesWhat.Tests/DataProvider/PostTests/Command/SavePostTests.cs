using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Ninject;
using Ninject.MockingKernel.Moq;
using NUnit.Framework;
using WhoOwesWhat.DataProvider;
using WhoOwesWhat.DataProvider.Entity;
using WhoOwesWhat.DataProvider.GroupEntity;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.DataProvider.PersonEntity;
using WhoOwesWhat.DataProvider.PostEntity;
using Post = WhoOwesWhat.DataProvider.Entity.Post;
using UserCredential = WhoOwesWhat.DataProvider.Entity.UserCredential;

namespace WhoOwesWhat.Tests.PostTests.Command
{

    [TestFixture]
    public class SavePostTests
    {
        private static MoqMockingKernel _kernel;

        public SavePostTests()
        {
            _kernel = new MoqMockingKernel();
            _kernel.Bind<IPostCommand>().To<PostCommand>();
            _kernel.Bind<ITestPostQuery>().To<TestPostQuery>();
        }

        private Mock<IWhoOwesWhatContext> _contextMock;

        [SetUp]
        public void SetUp()
        {
            _kernel.Reset();
            _contextMock = _kernel.GetMock<IWhoOwesWhatContext>();

        }

        [Test]
        public void When_saving_a_existing_post__it_should_update_and_save_changes()
        {
            var testPostQuery = _kernel.Get<ITestPostQuery>();

            var post = testPostQuery.GetEntityPost_Hooters();

            var personContext = _kernel.GetMock<IPersonContext>();
            personContext.Setup(mock => mock.GetPersonByPersonGuid(TestPersonQuery.GeirPersonEntity.PersonGuid)).Returns(TestPersonQuery.GeirPersonEntity);

            var postContext = _kernel.GetMock<IPostContext>();
            postContext.Setup(mock => mock.GetPostByPostGuid(It.IsAny<Guid>())).Returns(post);
            

            var groupContextMock = _kernel.GetMock<IGroupContext>();

            var post1 = testPostQuery.GetDomainPost_Hooters();

            var dataProvider = _kernel.Get<IPostCommand>();

            _contextMock.Setup(a => a.SaveChanges()).Returns(1);

            dataProvider.SavePost(post1);

            groupContextMock.Verify(a => a.GetGroupByGroupGuid(It.IsAny<Guid>()), Times.Never);
        }

        [Test]
        public void When_saving_a_new_post__it_should_create_new_post_and_save_changes()
        {
            var testPostQuery = _kernel.Get<ITestPostQuery>();

            var personContext = _kernel.GetMock<IPersonContext>();
            personContext.Setup(mock => mock.GetPersonByPersonGuid(TestPersonQuery.GeirPersonEntity.PersonGuid)).Returns(TestPersonQuery.GeirPersonEntity);
            
            var postContext = _kernel.GetMock<IPostContext>();
            postContext.Setup(mock => mock.GetPostByPostGuid(It.IsAny<Guid>())).Returns((Post) null);
            postContext.Setup(mock => mock.Add(It.IsAny<Post>()));

            var groupContext = _kernel.GetMock<IGroupContext>();

            var domainPost = testPostQuery.GetDomainPost_Hooters();

            var dataProvider = _kernel.Get<IPostCommand>();
            _contextMock.Setup(a => a.SaveChanges()).Returns(1);

            dataProvider.SavePost(domainPost);

        }             
        
        
        
    }
}

