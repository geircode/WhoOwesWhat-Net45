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
    public class UserControllerGetPersonTests
    {
        private static MoqMockingKernel _kernel;

        public UserControllerGetPersonTests()
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
        public void When_getting_an_user__it_should_succeed()
        {


            var queryMock = _kernel.GetMock<IUserCredentialQuery>();
            var person = new Person()
            {
                PersonGuid = new Guid("FAD0CC47-1337-1337-1337-0B34360C33FA"),
                Displayname = "Garg1337",
                Mobile = "13131313",
                IsDeleted = false
            };
            var credential = new UserCredential()
            {
                Email = "smurf@smurf.com",
                Username = "smurf@smurf.com",
                Person = person
            };
            queryMock.Setup(mock => mock.GetUserCredential(It.IsAny<string>())).Returns(credential);

            var userController = _kernel.Get<IUserController>(); // this will inject the mocked IBar into our normal MyFoo implementation
            GetPersonByEmailRequest request = new GetPersonByEmailRequest()
            {
                username = "Jolly",
                password = "Good",
                emailCriteria = "smurf@smurf.com"
            };

            //setup the mock
            var userCredentialRepositoryMock = _kernel.GetMock<IUserRepository>();
            userCredentialRepositoryMock.Setup(mock => mock.AuthenticateUser(request.username, request.password)).Returns(true);

            var response = userController.GetPersonByEmail(request);

            response.isSuccess.Should().BeTrue();
            response.displayname.Should().Be(person.Displayname);
            response.username.Should().Be(credential.Username);

            queryMock.VerifyAll();
        }        

       

    }
}

