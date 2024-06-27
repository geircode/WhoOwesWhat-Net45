using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using FluentAssertions;
using Moq;
using Ninject.MockingKernel.Moq;
using NUnit.Framework;
using WhoOwesWhat.DataProvider;
using WhoOwesWhat.DataProvider.Entity;
using WhoOwesWhat.DataProvider.FriendEntity;
using WhoOwesWhat.DataProvider.Interfaces;
using Ninject;


namespace WhoOwesWhat.Tests.FriendTests.Query
{

    [TestFixture]
    public class GetFriendsToAGetFriendsTests
    {
        private static MoqMockingKernel _kernel;
        Person _ownerPerson;
        private Mock<IWhoOwesWhatContext> _contextMock;

        public GetFriendsToAGetFriendsTests()
        {
            _kernel = new MoqMockingKernel();
            _kernel.Bind<IFriendQuery>().To<FriendQuery>();
            _kernel.Bind<IFriendDataProviderLogic>().To<FriendDataProviderLogic>();
        }

        [SetUp]
        public void SetUp()
        {
            _kernel.Reset();

            _ownerPerson = new DataProvider.Entity.Person()
            {
                PersonId = 1,
                PersonGuid = new Guid("11B7289D-1337-1337-1337-64E671D169C7"),
                Displayname = "Geir_Online"
            };

        }

        [Test]
        public void When_getting_Friends_to_a_Person__it_should_return_all_nondeleted_friends()
        {
            /*
             Action: GetFriends

             Before:
                Geir legger til Marianne som OfflineFriend på App, og synkroniserer
                Geir sender friendrequest til Beate
                Beate accepts
                Geir synkroniserer
             
             After:
             Geir får tilbake en liste med Beate og Marianne som Friends
             */

            var mariannePerson = new DataProvider.Entity.Person()
            {
                PersonId = 2,
                PersonGuid = new Guid("8B1B52A5-4747-4747-4747-8181B8EBC9E4"),
                Displayname = "Marianne_Offline"
            };

            var beatePerson = new DataProvider.Entity.Person()
            {
                PersonId = 3,
                PersonGuid = new Guid("8035804C-496C-4BAD-A568-098FA53C3416"),
                Displayname = "Beate_Online"
            };

            var friendGeirMarianne = new DataProvider.Entity.Friend()
            {
                Person = mariannePerson,
                Owner = _ownerPerson,
                IsDeleted = false
            };            
            
            var friendGeirBeate = new DataProvider.Entity.Friend()
            {
                Person = beatePerson,
                Owner = _ownerPerson,
                IsDeleted = false
            };

            var persons = new List<DataProvider.Entity.Person>();
            persons.Add(_ownerPerson);
            persons.Add(mariannePerson);
            persons.Add(beatePerson);

            IQueryable<DataProvider.Entity.Person> personsAsQueryable = persons.AsQueryable();

            var friends = new List<DataProvider.Entity.Friend>();
            friends.Add(friendGeirMarianne);
            friends.Add(friendGeirBeate);

            var friendsAsQueryable = friends.AsQueryable();

            _contextMock = _kernel.GetMock<IWhoOwesWhatContext>();

            var friendSqlMock = _kernel.GetMock<ISqlRepository<Friend>>();
            friendSqlMock.Setup(m => m.GetAll()).Returns(friendsAsQueryable);

            _contextMock.Setup(m => m.GetFriendSqlRepository()).Returns(friendSqlMock.Object);

            _contextMock.Setup(m => m.LoadProperty(It.IsAny<Friend>(), a => a.Person));

            var dataProvider = _kernel.Get<IFriendQuery>();

            var actualFriends = dataProvider.GetFriends(_ownerPerson.PersonGuid);

            var expectedFriends = new List<Domain.DTO.Friend>();
            expectedFriends.Add(new Domain.DTO.Friend()
            {
                FriendGuid = mariannePerson.PersonGuid,
                OwnerGuid = _ownerPerson.PersonGuid,
                IsDeleted = mariannePerson.IsDeleted
            });            
            expectedFriends.Add(new Domain.DTO.Friend()
            {
                FriendGuid = beatePerson.PersonGuid,
                OwnerGuid = _ownerPerson.PersonGuid,
                IsDeleted = beatePerson.IsDeleted
            });

            actualFriends.ShouldAllBeEquivalentTo(expectedFriends);
        }

        [Test]
        public void When_getting_Friends_to_a_Person__it_should_return_all_nondeleted_friends2()
        {
            /*
             Action: GetFriends

             Before:
                Geir legger til Marianne som OfflineFriend på App, og synkroniserer
                Geir sender friendrequest til Beate
                Beate accepts
                Geir synkroniserer
                Geir sletter Marianne og synkroniserer
             
             After:
             Geir får tilbake en liste med kun Beate som Friends
             */

            var mariannePerson = new DataProvider.Entity.Person()
            {
                PersonId = 2,
                PersonGuid = new Guid("8B1B52A5-4747-4747-4747-8181B8EBC9E4"),
                Displayname = "Marianne_Offline"
            };

            var beatePerson = new DataProvider.Entity.Person()
            {
                PersonId = 3,
                PersonGuid = new Guid("8035804C-496C-4BAD-A568-098FA53C3416"),
                Displayname = "Beate_Online"
            };

            var friendGeirMarianne = new DataProvider.Entity.Friend()
            {
                Person = mariannePerson,
                Owner = _ownerPerson,
                IsDeleted = true
            };

            var friendGeirBeate = new DataProvider.Entity.Friend()
            {
                Person = beatePerson,
                Owner = _ownerPerson,
                IsDeleted = false
            };

            var persons = new List<DataProvider.Entity.Person>();
            persons.Add(_ownerPerson);
            persons.Add(mariannePerson);
            persons.Add(beatePerson);

            IQueryable<DataProvider.Entity.Person> personsAsQueryable = persons.AsQueryable();

            var friends = new List<DataProvider.Entity.Friend>();
            friends.Add(friendGeirMarianne);
            friends.Add(friendGeirBeate);

            var friendsAsQueryable = friends.AsQueryable();

            _contextMock = _kernel.GetMock<IWhoOwesWhatContext>();

            var friendSqlMock = _kernel.GetMock<ISqlRepository<Friend>>();
            friendSqlMock.Setup(m => m.GetAll()).Returns(friendsAsQueryable);

            _contextMock.Setup(m => m.GetFriendSqlRepository()).Returns(friendSqlMock.Object);

            _contextMock.Setup(m => m.LoadProperty(It.IsAny<Friend>(), a => a.Person));

            var dataProvider = _kernel.Get<IFriendQuery>();

            var actualFriends = dataProvider.GetFriends(_ownerPerson.PersonGuid);

            var expectedFriends = new List<Domain.DTO.Friend>();
            expectedFriends.Add(new Domain.DTO.Friend()
            {
                FriendGuid = beatePerson.PersonGuid,
                OwnerGuid = _ownerPerson.PersonGuid,
                IsDeleted = beatePerson.IsDeleted
            });

            actualFriends.ShouldAllBeEquivalentTo(expectedFriends);
        }


    }
}

