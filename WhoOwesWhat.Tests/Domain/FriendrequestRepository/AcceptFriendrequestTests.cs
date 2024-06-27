using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Ninject;
using Ninject.MockingKernel.Moq;
using NSubstitute;
using NUnit.Framework;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.Domain;
using WhoOwesWhat.Domain.DTO;
using WhoOwesWhat.Domain.Friendrequest;
using WhoOwesWhat.Domain.Interfaces;

namespace WhoOwesWhat.Tests.FriendrequestRepository
{

    [TestFixture]
    public class AcceptFriendrequestTests
    {
        private static MoqMockingKernel _kernel;
        private Person _ownerPerson;

        public AcceptFriendrequestTests()
        {
            _kernel = new MoqMockingKernel();
            _kernel.Bind<IAcceptFriendrequestLogic>().To<AcceptFriendrequestLogic>();
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
        public void When_accepting_a_pending_friendrequests_to_a_person__it_should_succeed()
        {
            /*
             
                Action: Accept a friendrequest to a Person

                Before:
                Geir og Marianne er registerte brukere
                Marianne sender friendrequest til Geir
                Geir accepts the friendrequest
                
                After:
                Geir gets Marianne as a new Online Friend
                Marianne gets Geir as a new Online Friend
                
            */

            var friendMarianne = new Person()
            {
                PersonGuid = new Guid("14DD5F5A-4747-4747-4747-64A821B1EAA0"),
                Displayname = "Marianne",
                IsDeleted = false
            };
            var marianneCredential = new UserCredential()
            {
                Email = "marianne@moo.com",
                Username = "marianne@moo.com",
                Person = friendMarianne
            };

            var friendBeate = new Person()
            {
                PersonGuid = new Guid("651FEC7D-1C43-4099-AF11-A8E59EA2BAE5"),
                Displayname = "Beate",
                IsDeleted = false
            };

            var beateCredential = new UserCredential()
            {
                Email = "beate@moo.com",
                Username = "beate@moo.com",
                Person = friendBeate
            };

            const string geirUsername = "smurf@smurf.com";
            const string friendUsername = "marianne@moo.com";

            var personQueryMock = _kernel.GetMock<IPersonQuery>();
            personQueryMock.Setup(mock => mock.GetPersonByUsername(geirUsername)).Returns(_ownerPerson);
            personQueryMock.Setup(mock => mock.GetPersonByUsername(friendUsername)).Returns(friendMarianne);


            var friendQueryMock = _kernel.GetMock<IFriendQuery>();
            List<Friend> friends = new List<Friend>();
            friends.Add(new Friend() { FriendGuid = friendBeate.PersonGuid, OwnerGuid = _ownerPerson.PersonGuid, IsDeleted = false });
            friendQueryMock.Setup(mock => mock.GetFriends(_ownerPerson.PersonGuid)).Returns(friends);


            var friendrequestQueryMock = _kernel.GetMock<IFriendrequestQuery>();
            friendrequestQueryMock.Setup(mock => mock.ExistsFriendrequest(_ownerPerson.PersonGuid, friendMarianne.PersonGuid)).Returns(true);

            var friendCommandMock = _kernel.GetMock<IFriendCommand>();
            Friend newFriendGeirMarianne = new Friend()
            {
                FriendGuid = friendMarianne.PersonGuid,
                OwnerGuid = _ownerPerson.PersonGuid
            };            
            Friend newFriendMarianneGeir = new Friend()
            {
                FriendGuid = _ownerPerson.PersonGuid,
                OwnerGuid = friendMarianne.PersonGuid
            };
            // NB: jeg burde kanskje dele dette opp i to funkjoner. En for hver vei. Evt redesign av Friendship

            friendCommandMock.Setup(mock => mock.SaveFriend(It.IsAny<Friend>()));
            friendCommandMock.Setup(mock => mock.SaveFriend(It.IsAny<Friend>()));

            var friendrequestCommandMock = _kernel.GetMock<IFriendrequestCommand>();
            friendrequestCommandMock.Setup(mock => mock.DeleteFriendrequest(_ownerPerson.PersonGuid, friendMarianne.PersonGuid));

            var acceptFriendrequestLogic = _kernel.Get<IAcceptFriendrequestLogic>();
            acceptFriendrequestLogic.AcceptFriendrequest(geirUsername, friendUsername);

            personQueryMock.VerifyAll();
            friendrequestQueryMock.VerifyAll();
            friendCommandMock.VerifyAll();
            friendQueryMock.VerifyAll();
            friendrequestCommandMock.VerifyAll();
        }

        [Test]
        public void When_accepting_a_pending_friendrequests_to_a_person__it_should_fail()
        {
            // because the Person is deleted
            // TODO: what to do when the Person is Deleted? A Person can not be deleted if it has Friendrequests. Else delete these first.
        }  
        
    }
}

