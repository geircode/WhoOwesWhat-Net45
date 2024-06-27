using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Ninject;
using Ninject.MockingKernel.Moq;
using NUnit.Framework;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.Domain;
using WhoOwesWhat.Domain.DTO;
using WhoOwesWhat.Domain.Friend;
using WhoOwesWhat.Domain.Interfaces;

namespace WhoOwesWhat.Tests.FriendRepository
{

    [TestFixture]
    public class GetFriendsToAppTests
    {
        private static MoqMockingKernel _kernel;
        Person _ownerPerson;

        public GetFriendsToAppTests()
        {
            _kernel = new MoqMockingKernel();
            _kernel.Bind<IFriendRepository>().To<Domain.Friend.FriendRepository>();
        }

        [SetUp]
        public void SetUp()
        {
            _kernel.Reset();

            _ownerPerson = new Person()
            {
                PersonGuid = new Guid("FAD0CC47-1337-1337-1337-0B34360C33FA"),
                Displayname = "Garg1337",
                IsDeleted = false
            };
        }

        [Test]
        public void When_getting_the_FriendsSyncmodel_for_the_App()
        {
            /*
             Action: GetFriendsToApp

             Before:
                Geir legger til Marianne som OfflineFriend på App(Device Id:1)
                Geir synkroniserer App(Device Id:1)
                Geir logger inn på App(Device Id:2)
                Geir synkroniserer App(Device Id:2)
             
             After:
             Geir sin andre Device(id:2) får Marianne som OfflinePerson

             */

            var friendMarianne = new Person()
            {
                PersonGuid = new Guid("14DD5F5A-4747-4747-4747-64A821B1EAA0"),
                Displayname = "Marianne",
                IsDeleted = false
            };

            var friendBeate = new Person()
            {
                PersonGuid = new Guid("651FEC7D-1C43-4099-AF11-A8E59EA2BAE5"),
                Displayname = "Beate",
                IsDeleted = false
            };

            const string username = "smurf@smurf.com";

            var personQueryMock = _kernel.GetMock<IPersonQuery>();
            personQueryMock.Setup(mock => mock.GetPersonByUsername(username)).Returns(_ownerPerson);
            

            var friendQueryMock = _kernel.GetMock<IFriendQuery>();
            List<Friend> friends = new List<Friend>();
            friends.Add(new Friend() { FriendGuid = friendMarianne.PersonGuid, OwnerGuid = _ownerPerson.PersonGuid, IsDeleted = false });
            friends.Add(new Friend() { FriendGuid = friendBeate.PersonGuid, OwnerGuid = _ownerPerson.PersonGuid, IsDeleted = false });
            friendQueryMock.Setup(mock => mock.GetFriendsIncludeDeleted(_ownerPerson.PersonGuid)).Returns(friends);

            var credentialQueryMock = _kernel.GetMock<IUserCredentialQuery>();
            credentialQueryMock.Setup(mock => mock.IsOnlinePerson(friendMarianne.PersonGuid)).Returns(false);
            credentialQueryMock.Setup(mock => mock.IsOnlinePerson(friendBeate.PersonGuid)).Returns(false);

            personQueryMock.Setup(mock => mock.GetPerson(friendMarianne.PersonGuid)).Returns(friendMarianne);
            personQueryMock.Setup(mock => mock.GetPerson(friendBeate.PersonGuid)).Returns(friendBeate);

            var friendRepository = _kernel.Get<IFriendRepository>();
            var modelList = friendRepository.GetFriendsToApp(username);

            personQueryMock.VerifyAll();
            friendQueryMock.VerifyAll();
            credentialQueryMock.VerifyAll();

            var expectedModelList = new List<GetFriendsToAppModel>();
            expectedModelList.Add(new GetFriendsToAppModel()
            {
                Displayname = friendMarianne.Displayname,
                FriendGuid = friendMarianne.PersonGuid,
                IsFriendDeleted = friendMarianne.IsDeleted,
                IsFriendOnlinePerson = false,
                IsPersonDeleted = false,
                OwnerGuid = _ownerPerson.PersonGuid
            });            
            expectedModelList.Add(new GetFriendsToAppModel()
            {
                Displayname = friendBeate.Displayname,
                FriendGuid = friendBeate.PersonGuid,
                IsFriendDeleted = friendBeate.IsDeleted,
                IsFriendOnlinePerson = false,
                IsPersonDeleted = false,
                OwnerGuid = _ownerPerson.PersonGuid
            });

            modelList.ShouldAllBeEquivalentTo(expectedModelList);
        }


    }
}

