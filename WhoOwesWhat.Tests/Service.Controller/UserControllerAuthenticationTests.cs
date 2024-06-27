using System;
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
    public class UserControllerAuthenticationTests
    {
        private static MoqMockingKernel _kernel;

        public UserControllerAuthenticationTests()
        {
            _kernel = new MoqMockingKernel();
            _kernel.Bind<IUserController>().To<UserController>();
        }

        [SetUp]
        public void SetUp()
        {
            _kernel.Reset();
        }

        [Test]
        public void When_authenticating_an_user__it_should_succeed()
        {
            //setup the mock
            var userCredentialRepositoryMock = _kernel.GetMock<IUserRepository>();
            userCredentialRepositoryMock.Setup(mock => mock.AuthenticateUser(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            var _personQueryMock = _kernel.GetMock<IPersonQuery>();
            var person = new Person()
            {
                PersonGuid = new Guid("FAD0CC47-1337-1337-1337-0B34360C33FA"),
                Displayname = "Garg1337",
                Mobile = "13131313",
                IsDeleted = false
            };
            _personQueryMock.Setup(mock => mock.GetPersonByUsername(It.IsAny<string>())).Returns(person);

            var userController = _kernel.Get<IUserController>(); // this will inject the mocked IBar into our normal MyFoo implementation
            var request = new AuthenticateUserRequest()
            {
                username = "Jolly",
                password = "Good"
            };
            var authenticateUserResponse = userController.AuthenticateUser(request);
            authenticateUserResponse.isSuccess.Should().Be(true);
            authenticateUserResponse.displayname.Should().Be(person.Displayname);
            authenticateUserResponse.mobil.Should().Be(person.Mobile);
            authenticateUserResponse.personGuid.Should().Be(person.PersonGuid);

            userCredentialRepositoryMock.VerifyAll();

            // expect(_personRepository.IsUniquePersonGuid).tobecalledwith(user.PersonGuid);
        }        
        
        //[Test]
        //public void When_authenticating_an_user_with_a_nonexisting_user__it_should_fail()
        //{
        //    //setup the mock
        //    var userCredentialRepositoryMock = _kernel.GetMock<IUserRepository>();

        //    var userController = _kernel.Get<IUserController>();
        //    var request = new AuthenticateUserRequest()
        //    {
        //        username = "Jolly",
        //        password = "Good"
        //    };
        //    var authenticateUserResponse = userController.AuthenticateUser(request);
        //    authenticateUserResponse.isSuccess.Should().Be(false);
        //    userCredentialRepositoryMock.Verify(a => a.AuthenticateUser(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
        //}        
        
        [Test]
        public void When_authenticating_an_user_with_wrong_password__it_should_fail()
        {
            //setup the mock
            var userCredentialRepositoryMock = _kernel.GetMock<IUserRepository>();
            userCredentialRepositoryMock.Setup(mock => mock.AuthenticateUser(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            var userController = _kernel.Get<IUserController>();
            var request = new AuthenticateUserRequest()
            {
                username = "Jolly",
                password = "Good"
            };
            var authenticateUserResponse = userController.AuthenticateUser(request);
            authenticateUserResponse.isSuccess.Should().Be(false);
        }

       

    }
}

