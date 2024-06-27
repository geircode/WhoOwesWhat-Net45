using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Ninject;
using Ninject.MockingKernel.Moq;
using NUnit.Framework;
using WhoOwesWhat.DataProvider;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.DataProvider.PersonEntity;
using Person = WhoOwesWhat.DataProvider.Entity.Person;
using UserCredential = WhoOwesWhat.DataProvider.Entity.UserCredential;

namespace WhoOwesWhat.Tests.PersonTests.Command
{

    [TestFixture]
    public class SavePersonTests
    {
        private static MoqMockingKernel _kernel;

        public SavePersonTests()
        {
            _kernel = new MoqMockingKernel();
            _kernel.Bind<IPersonCommand>().To<PersonCommand>();
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

        
        
    }
}

