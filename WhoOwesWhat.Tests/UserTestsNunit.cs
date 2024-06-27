using System;
using FluentAssertions;
using Moq;
using Ninject;
using Ninject.MockingKernel.Moq;
using NUnit.Framework;
using WhoOwesWhat.Domain.DTO;
using WhoOwesWhat.Domain.Interfaces;
using WhoOwesWhat.Service.Controller;
using WhoOwesWhat.Service.DTO;

namespace WhoOwesWhat.Tests
{

    [TestFixture]
    public class UserTestsNunit
    {
        private static MoqMockingKernel _kernel;

        public UserTestsNunit()
        {
            _kernel = new MoqMockingKernel();
            _kernel.Bind<IUserController>().To<UserController>();
        }

        [SetUp]
        public void SetUp()
        {
            _kernel.Reset();
        }

        //[Test]
        //public void When_authenticating_a_user__it_should_return_success()
        //{
        //    //setup the mock
        //    var barMock = _kernel.GetMock<IUserRepository>();
        //    barMock.Setup(mock => mock.AuthenticateUser(It.IsAny<string>(), It.IsAny<string>())).Returns(new UserCredential());

        //    var controller = _kernel.Get<IUserController>(); // this will inject the mocked IBar into our normal MyFoo implementation
        //    var result = controller.AuthenticateUser(new AuthenticateUserRequest());  // this will print "mocked Content", because the mocked object is called
        //    result.isSuccess.Should().BeTrue();

        //    barMock.VerifyAll();
        //}        
        
        //[Test]
        //public void When_authenticating_user_should_return_success2()
        //{
        //    //setup the mock

        //    var controller = _kernel.Get<IUserController>(); // this will inject the mocked IBar into our normal MyFoo implementation
        //    var result = controller.AuthenticateUser(new AuthenticateUserRequest());  // this will print "mocked Content", because the mocked object is called
        //    result.isSuccess.Should().BeTrue();
        //}

        [Test]
        public void When_creating_a_user__it_should_return_success()
        {
            //setup the mock
            var barMock = _kernel.GetMock<IUserRepository>();
            barMock.Setup(mock => mock.CreateUser(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            var controller = _kernel.Get<IUserController>(); // this will inject the mocked IBar into our normal MyFoo implementation
            var result = controller.CreateUser(new CreateUserRequest());
            result.isSuccess.Should().BeTrue();

            barMock.VerifyAll();

            // expect(_personRepository.IsUniquePersonGuid).tobecalledwith(PersonGuid);
        }   
    }
}

