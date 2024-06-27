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
    public class UserRepositoryLogicTests
    {
        private static MoqMockingKernel _kernel;

        public UserRepositoryLogicTests()
        {
            _kernel = new MoqMockingKernel();
            _kernel.Bind<IUserRepositoryLogic>().To<UserRepositoryLogic>();
        }

        [SetUp]
        public void SetUp()
        {
            _kernel.Reset();
        }

        [Test]
        public void When_creating_a_user__it_should_map_correctly()
        {
            Guid personGuid = new Guid("E113B60A-3A4B-4074-978C-7CF29FF18352");
            const string displayname = "Garg1337";
            const string username = "test@test.com";
            const string email = "test@test.com";
            const string mobile = "80015000";
            const string password = "asdasd";
            const string passwordHash = "A8F5F167F44F4964E6C998DEE827110C"; // asdasd

            var person = new Domain.DTO.Person()
            {
                PersonGuid = personGuid,
                Displayname = displayname,
                Mobile = mobile
            };


            var logic = _kernel.Get<IUserRepositoryLogic>(); // this will inject the mocked IBar into our normal MyFoo implementation
            var user = logic.MapToAddUserCredentialModel(personGuid, username, email, passwordHash);
            user.PersonGuid.Should().Be(personGuid);
            user.PasswordHash.Should().Be(passwordHash);
            user.Username.Should().Be(username);
            user.Email.Should().Be(email);
        }


    }
}

