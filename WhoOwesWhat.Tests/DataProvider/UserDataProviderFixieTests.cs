//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using FluentAssertions;
//using Moq;
//using Ninject;
//using Ninject.MockingKernel.Moq;
//using NUnit.Framework;
//using WhoOwesWhat.DataProvider;
//using WhoOwesWhat.DataProvider.Entity;
//using WhoOwesWhat.DataProvider.Interfaces;
//using WhoOwesWhat.Service.Controller;
//using WhoOwesWhat.Service.DTO;

//namespace WhoOwesWhat.Tests.DataProvider
//{

//    public class UserDataProviderFixieTests
//    {
//        private static MoqMockingKernel _kernel;

//        public UserDataProviderFixieTests()
//        {
//            _kernel = new MoqMockingKernel();
//            _kernel.Bind<IUserCredentialQuery>().To<UserCredentialCredentialDataProvider>();
//        }

//        public void SetUp()
//        {
//            _kernel.Reset();
//        }

//        public void When_getting_a_user_it_should_return_a_mapped_DomainUser()
//        {

//            var data = new List<UserCredential> 
//            { 
//                new UserCredential { PersonId = 1, Displayname = "BBB", Email = "asd1@hotmail.com", PasswordHash = "asdasdasdasdasd"}, 
//                new UserCredential { PersonId = 2, Displayname = "XXX", Email = "asd2@hotmail.com", PasswordHash = "asdasdasdasdasd"}, 
//                new UserCredential { PersonId = 3, Displayname = "ZZZ", Email = "asd3@hotmail.com", PasswordHash = "asdasdasdasdasd"}, 
//            }.AsQueryable();

//            var mockSet = new Mock<DbSet<UserCredential>>();
//            mockSet.As<IQueryable<UserCredential>>().Setup(m => m.Provider).Returns(data.Provider);
//            mockSet.As<IQueryable<UserCredential>>().Setup(m => m.Expression).Returns(data.Expression);
//            mockSet.As<IQueryable<UserCredential>>().Setup(m => m.ElementType).Returns(data.ElementType);
//            mockSet.As<IQueryable<UserCredential>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

//            //var mockContext = new Mock<WhoOwesWhatContext>();
//            var barMock = _kernel.GetMock<IWhoOwesWhatContext>();
//            barMock.Setup(m => m.UserCredentials).Returns(mockSet.Object);
//            //barMock.Setup(mock => mock.UserCredentials.Returns(new UserCredential() { Displayname = "Garg!" });

//            var dataProvider = _kernel.Get<IUserCredentialQuery>(); // this will inject the mocked IBar into our normal MyFoo implementation
//            var result = dataProvider.GetUsersByName("BBB");
//            result.Should().NotBeNull();
//            result.ShouldBeEquivalentTo(new Domain.DTO.UserCredential { Id = 1, Name = "BBB", Email = "asd1@hotmail.com", PasswordHash = "asdasdasdasdasd" });
//        }

//        public void When_getting_users_it_should_return_a_list_of_DomainUsers()
//        {
//            //setup the mock

//            var data = new List<UserCredential> 
//            { 
//                new UserCredential { PersonId = 1, Displayname = "BBB", Email = "asd1@hotmail.com", PasswordHash = "asdasdasdasdasd"}, 
//                new UserCredential { PersonId = 2, Displayname = "XXX", Email = "asd2@hotmail.com", PasswordHash = "asdasdasdasdasd"}, 
//                new UserCredential { PersonId = 3, Displayname = "ZZZ", Email = "asd3@hotmail.com", PasswordHash = "asdasdasdasdasd"}, 
//            }.AsQueryable();

//            var mockSet = new Mock<DbSet<UserCredential>>();
//            mockSet.As<IQueryable<UserCredential>>().Setup(m => m.Provider).Returns(data.Provider);
//            mockSet.As<IQueryable<UserCredential>>().Setup(m => m.Expression).Returns(data.Expression);
//            mockSet.As<IQueryable<UserCredential>>().Setup(m => m.ElementType).Returns(data.ElementType);
//            mockSet.As<IQueryable<UserCredential>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

//            //var mockContext = new Mock<WhoOwesWhatContext>();
//            var barMock = _kernel.GetMock<IWhoOwesWhatContext>();
//            barMock.Setup(m => m.UserCredentials).Returns(mockSet.Object); 
//            //barMock.Setup(mock => mock.UserCredentials.Returns(new UserCredential() { Displayname = "Garg!" });

//            var dataProvider = _kernel.Get<IUserCredentialQuery>(); // this will inject the mocked IBar into our normal MyFoo implementation
//            var result = dataProvider.GetUserCredentials();
//            result.Count.Should().Be(3);
//        }

//    }
//}

