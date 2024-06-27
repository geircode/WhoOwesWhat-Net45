using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
using FluentAssertions;
using Moq;
using Ninject;
using Ninject.MockingKernel.Moq;
using NUnit.Framework;
using WhoOwesWhat.DataProvider;
using WhoOwesWhat.DataProvider.Entity;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.DataProvider.PersonEntity;
using WhoOwesWhat.DataProvider.UserCredentialEntity;

namespace WhoOwesWhat.Tests
{

    [TestFixture]
    public class UserCredentialDataproviderGetUserCredentialsTests
    {
        private static MoqMockingKernel _kernel;

        public UserCredentialDataproviderGetUserCredentialsTests()
        {
            _kernel = new MoqMockingKernel();
            _kernel.Bind<IUserCredentialQuery>().To<UserCredentialQuery>();
        }

        private Mock<IWhoOwesWhatContext> _contextMock;

        [SetUp]
        public void SetUp()
        {
            _kernel.Reset();

            var person1 = new Person()
            {
                PersonId = 1,
                PersonGuid = new Guid("11B7289D-4848-4F1C-A905-64E671D169C7"),
                Displayname = "Geir1"
            };

            var person2 = new Person()
            {
                PersonId = 2,
                PersonGuid = new Guid("8B1B52A5-18A7-45CE-940C-8181B8EBC9E4"),
                Displayname = "Geir2"
            };

            var person3 = new Person()
            {
                PersonId = 3,
                PersonGuid = new Guid("8035804C-496C-4BAD-A568-098FA53C3416"),
                Displayname = "Geir3"
            };


            var persons = new List<Person>();
            persons.Add(person1);
            persons.Add(person2);
            persons.Add(person3);

            IQueryable<Person> personsAsQueryable = persons.AsQueryable();
            IQueryable<UserCredential> userCredentialAsQueryable = new List<UserCredential> 
            { 
                new UserCredential { PersonId = 1, Username = "BBB", Email = "asd1@hotmail.com", PasswordHash = "asdasdasdasdasd", Person = person1}, 
                new UserCredential { PersonId = 2, Username = "FFF", Email = "asd2@hotmail.com", PasswordHash = "asdasdasdasdasd", Person = person2}, 
                new UserCredential { PersonId = 3, Username = "XXX", Email = "asd3@hotmail.com", PasswordHash = "asdasdasdasdasd", Person = person3}, 
            }.AsQueryable();

            _contextMock = _kernel.GetMock<IWhoOwesWhatContext>();

            var userCredentialSqlMock = _kernel.GetMock<ISqlRepository<UserCredential>>();
            userCredentialSqlMock.Setup(m => m.GetAll()).Returns(userCredentialAsQueryable);

            _contextMock.Setup(m => m.GetUserCredentialSqlRepository()).Returns(userCredentialSqlMock.Object);

            var personSqlMock = _kernel.GetMock<ISqlRepository<Person>>();
            personSqlMock.Setup(m => m.GetAll()).Returns(personsAsQueryable);

            _contextMock.Setup(m => m.GetPersonSqlRepository()).Returns(personSqlMock.Object);
        }

        [Test]
        public void When_getting_an_existing_user__it_should_succeed()
        {
            var userDb = new DataProvider.Entity.UserCredential();
            var userDbContext = _kernel.GetMock<IUserCredentialContext>();
            userDbContext.Setup(a => a.GetUserCredential("BBB")).Returns(userDb);

            var personDb = new DataProvider.Entity.Person();
            var personContext = _kernel.GetMock<IPersonContext>();
            personContext.Setup(a => a.GetPerson(userDb.PersonId)).Returns(personDb);

            var dataProviderLogic = _kernel.GetMock<IUserCredentialDataProviderLogic>();
            dataProviderLogic.Setup(a => a.MapToDomain(userDb)).Returns(new Domain.DTO.UserCredential());

            var dataProvider = _kernel.Get<IUserCredentialQuery>(); // this will inject the mocked IBar into our normal MyFoo implementation
            var result = dataProvider.GetUserCredential("BBB");

            result.Should().NotBeNull();
            dataProviderLogic.VerifyAll();
        }        
        
        [Test]
        public void When_getting_an_nonexisting_user__it_should_succeed()
        {
            var dataProvider = _kernel.Get<IUserCredentialQuery>();

            var result = dataProvider.GetUserCredential("JollyGood");

            result.Should().Be(null);
        }

        [Test]
        public void When_getting_users_it_should_return_a_list_of_DomainUsers()
        {
            var dataProvider = _kernel.Get<IUserCredentialQuery>(); // this will inject the mocked IBar into our normal MyFoo implementation
            var result = dataProvider.GetUserCredentials();
            result.Count.Should().Be(3);
        }


    }
}

