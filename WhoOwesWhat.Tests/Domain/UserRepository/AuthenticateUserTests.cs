using System;
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


namespace WhoOwesWhat.Tests
{

    [TestFixture]
    public class AuthenticateUserTests
    {
        private static MoqMockingKernel _kernel;

        public AuthenticateUserTests()
        {
            _kernel = new MoqMockingKernel();
            _kernel.Bind<IUserRepository>().To<UserRepository>();
            _kernel.Bind<IHashUtils>().To<HashUtils>();
        }

        [SetUp]
        public void SetUp()
        {
            _kernel.Reset();
        }

        [Test]
        public void When_authenticating_an_user__it_should_succeed()
        {
            var person = new Person()
            {
                PersonGuid = new Guid("8035804C-496C-4BAD-A568-098FA53C3416"),
                Displayname = "Geir3"
            };

           var user =  new UserCredential
           {
               Username = "asd3@hotmail.com", 
               Email = "asd3@hotmail.com",
               PasswordHash = "0C6AD70BEB3A7E76C3FC7ADAB7C46ACC",  // Good
               Person = person
           };
            

            //setup the mock
            var userCredentialRepositoryMock = _kernel.GetMock<IUserCredentialQuery>();
            userCredentialRepositoryMock.Setup(mock => mock.GetUserCredential("asd3@hotmail.com")).Returns(user);

            var userRepository = _kernel.Get<IUserRepository>(); // this will inject the mocked IBar into our normal MyFoo implementation

            bool isAuthenticated = userRepository.AuthenticateUser("asd3@hotmail.com", "Good");
            isAuthenticated.Should().BeTrue();

            userCredentialRepositoryMock.VerifyAll();
        }

        [Test]
        public void When_authenticating_an_user_that_is_null__it_should_fail()
        {
            //setup the mock
            var userCredentialRepositoryMock = _kernel.GetMock<IUserCredentialQuery>();
            userCredentialRepositoryMock.Setup(mock => mock.GetUserCredential("asd3@hotmail.com")).Returns((UserCredential) null);

            var userRepository = _kernel.Get<IUserRepository>(); // this will inject the mocked IBar into our normal MyFoo implementation

            bool isAuthenticated = userRepository.AuthenticateUser("asd3@hotmail.com", "Good");
            isAuthenticated.Should().BeFalse();

            userCredentialRepositoryMock.VerifyAll();
        }

    }
}

