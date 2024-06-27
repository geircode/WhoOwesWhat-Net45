using System;
using FluentAssertions;
using Ninject;
using Ninject.MockingKernel.Moq;
using NUnit.Framework;
using WhoOwesWhat.DataProvider;
using WhoOwesWhat.DataProvider.PersonEntity;

namespace WhoOwesWhat.Tests.PersonTests
{

    [TestFixture]
    public class PersonDataproviderLogicTests
    {
        private static MoqMockingKernel _kernel;

        public PersonDataproviderLogicTests()
        {
            _kernel = new MoqMockingKernel();
            _kernel.Bind<IPersonDataProviderLogic>().To<PersonDataProviderLogic>();
        }

        [SetUp]
        public void SetUp()
        {
            _kernel.Reset();

        }

        [Test]
        public void When_mapping_to_Domain__it_should_succeed()
        {
            var personDb = new DataProvider.Entity.Person()
            {
                PersonId = 1,
                PersonGuid = new Guid("11B7289D-4848-4F1C-A905-64E671D169C7"),
                Displayname = "Geir1",
                Mobile = "800555111",
                IsDeleted = false
            };            
            
            var personDomain = new Domain.DTO.Person()
            {
                PersonGuid = new Guid("11B7289D-4848-4F1C-A905-64E671D169C7"),
                Displayname = "Geir1",
                Mobile = "800555111",
                IsDeleted = false
            };


            var dataProvider = _kernel.Get<IPersonDataProviderLogic>();

            var result = dataProvider.MapToDomain(personDb);

            result.ShouldBeEquivalentTo(personDomain);
        }             
        
        [Test]
        public void When_updating_database_entity__it_should_succeed()
        {
            var personDb = new DataProvider.Entity.Person()
            {
                PersonId = 1,
                PersonGuid = new Guid("11B7289D-4848-4F1C-A905-64E671D169C7"),
                Displayname = "Some other name",
                Mobile = "13371337",
                IsDeleted = true
            };            
            
            var personDomain = new Domain.DTO.Person()
            {
                PersonGuid = new Guid("11B7289D-4848-4F1C-A905-64E671D169C7"),
                Displayname = "Geir1",
                Mobile = "800555111",
                IsDeleted = false
            };

            personDb.ShouldBeEquivalentTo(personDb);
        }          
        
           
        
    }
}

