using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using Ninject;
using Ninject.MockingKernel.Moq;
using NUnit.Framework;
using WhoOwesWhat.DataProvider;
using WhoOwesWhat.DataProvider.Entity;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.DataProvider.PersonEntity;

namespace WhoOwesWhat.Tests.PersonTests.Query
{

    [TestFixture]
    public class GetPersonTests
    {
        private static MoqMockingKernel _kernel;
        private Mock<IWhoOwesWhatContext> _contextMock;

        public GetPersonTests()
        {
            _kernel = new MoqMockingKernel();
            _kernel.Bind<IPersonQuery>().To<PersonQuery>();
        }

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
        public void When_getting_a_existing_person__it_should_succeed()
        {
            var dataProvider = _kernel.Get<IPersonQuery>();

            var personRepositoryTestMock = _kernel.GetMock<IPersonDataProviderLogic>();
            personRepositoryTestMock.Setup(mock => mock.MapToDomain(It.IsAny<DataProvider.Entity.Person>())).Returns(new Domain.DTO.Person());

            var result = dataProvider.GetPerson(new Guid("8B1B52A5-18A7-45CE-940C-8181B8EBC9E4"));
            result.Should().NotBeNull();
            personRepositoryTestMock.VerifyAll();
        }             
        
        [Test]
        public void When_getting_a_momexisting_person__it_should_return_null()
        {
            var dataProvider = _kernel.Get<IPersonQuery>();

            var personRepositoryTestMock = _kernel.GetMock<IPersonDataProviderLogic>();

            var result = dataProvider.GetPerson(new Guid("FC16C853-94F8-42CB-929F-58770C38B71E"));
            result.Should().BeNull();
            
            personRepositoryTestMock.VerifyAll();
        }          
        
        
    }
}

