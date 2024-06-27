using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using FluentAssertions;
using Moq;
using Ninject;
using Ninject.MockingKernel.Moq;
using NUnit.Framework;
using WhoOwesWhat.DataProvider;
using WhoOwesWhat.DataProvider.Entity;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.DataProvider.UserCredentialEntity;
using WhoOwesWhat.Domain;
using WhoOwesWhat.Domain.Interfaces;

namespace WhoOwesWhat.Tests
{

    [TestFixture]
    public class UserCredentialDataproviderLogicTests
    {
        private static MoqMockingKernel _kernel;

        public UserCredentialDataproviderLogicTests()
        {
            _kernel = new MoqMockingKernel();
            _kernel.Bind<IUserCredentialDataProviderLogic>().To<UserCredentialDataProviderLogic>();
            _kernel.Bind<IUserRepositoryLogic>().To<UserRepositoryLogic>();
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

            var userRepositoryLogic = _kernel.Get<IUserRepositoryLogic>();
            var userDomain = userRepositoryLogic.MapToAddUserCredentialModel(personGuid, username, email, passwordHash);

            var logic = _kernel.Get<IUserCredentialDataProviderLogic>();
            var user = new UserCredential();
            user.Person = new Person();

            user.PasswordHash.Should().Be(passwordHash);
            user.Username.Should().Be(username);
            user.Email.Should().Be(email);
        }

    }
}

