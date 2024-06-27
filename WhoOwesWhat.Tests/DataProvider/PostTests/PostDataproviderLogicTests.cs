//using System;
//using FluentAssertions;
//using Ninject;
//using Ninject.MockingKernel.Moq;
//using NUnit.Framework;
//using WhoOwesWhat.DataProvider;
//using WhoOwesWhat.DataProvider.PostEntity;

//namespace WhoOwesWhat.Tests.PostTests
//{

//    [TestFixture]
//    public class PostDataproviderLogicTests
//    {
//        private static MoqMockingKernel _kernel;

//        public PostDataproviderLogicTests()
//        {
//            _kernel = new MoqMockingKernel();
//            _kernel.Bind<IPostDataProviderLogic>().To<PostDataProviderLogic>();
//        }

//        [SetUp]
//        public void SetUp()
//        {
//            _kernel.Reset();

//        }

//        [Test]
//        public void When_mapping_to_Domain__it_should_succeed()
//        {
//            var personDb = new DataProvider.Entity.Post()
//            {
//                PostId = 1,
//                PostGuid = new Guid("11B7289D-4848-4F1C-A905-64E671D169C7"),
//                IsDeleted = false
//            };            
            
//            var personDomain = new Domain.DTO.Post()
//            {
//                PostGuid = new Guid("11B7289D-4848-4F1C-A905-64E671D169C7"),
//                IsDeleted = false
//            };


//            var dataProvider = _kernel.Get<IPostDataProviderLogic>();

//            var result = dataProvider.MapToDomain(personDb);

//            result.ShouldBeEquivalentTo(personDomain);
//        }             
        
//        [Test]
//        public void When_updating_database_entity__it_should_succeed()
//        {
//            var personDb = new DataProvider.Entity.Post()
//            {
//                PostId = 1,
//                PostGuid = new Guid("11B7289D-4848-4F1C-A905-64E671D169C7"),
//                Displayname = "Some other name",
//                Mobile = "13371337",
//                IsDeleted = true
//            };            
            
//            var personDomain = new Domain.DTO.Post()
//            {
//                PostGuid = new Guid("11B7289D-4848-4F1C-A905-64E671D169C7"),
//                Displayname = "Geir1",
//                Mobile = "800555111",
//                IsDeleted = false
//            };


//            var dataProvider = _kernel.Get<IPostDataProviderLogic>();

//            dataProvider.UpdateEntity(personDomain, personDb);

//            personDb.ShouldBeEquivalentTo(personDb);
//        }          
        
           
        
//    }
//}

