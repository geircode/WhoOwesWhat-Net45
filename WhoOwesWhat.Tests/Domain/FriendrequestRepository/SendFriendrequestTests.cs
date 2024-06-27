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
using WhoOwesWhat.Domain.Friendrequest;
using WhoOwesWhat.Domain.Interfaces;

namespace WhoOwesWhat.Tests.FriendrequestRepository
{

    [TestFixture]
    public class SendFriendrequestTests
    {
        private static MoqMockingKernel _kernel;
        private Person _ownerPerson;

        public SendFriendrequestTests()
        {
            _kernel = new MoqMockingKernel();
            _kernel.Bind<IFriendrequestRepository>().To<Domain.Friendrequest.FriendrequestRepository>();
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
        public void When_sending_a_friendrequest__it_should_succeed()
        {
            /*
             
                Action: Send friendrequest

                Before:
                Geir legger til Marianne og Beate som Online friends
                Geir sender friendrequest til Victor
                
                After:
                Ny friendrequest mellom Geir og Victor blir lagret

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

            var friendVictorNew = new Person()
            {
                PersonGuid = new Guid("B0C5D73B-F0E7-4882-A21C-7B3D5ED9AE93"),
                Displayname = "Victor",
                IsDeleted = false
            };

            const string username = "smurf@smurf.com";
            const string friendUsername = "victor@wizz.com";

            var personQueryMock = _kernel.GetMock<IPersonQuery>();
            personQueryMock.Setup(mock => mock.GetPersonByUsername(username)).Returns(_ownerPerson);
            personQueryMock.Setup(mock => mock.GetPersonByUsername(friendUsername)).Returns(friendVictorNew);


            var friendQueryMock = _kernel.GetMock<IFriendQuery>();
            List<Friend> friends = new List<Friend>();
            friends.Add(new Friend() { FriendGuid = friendMarianne.PersonGuid, OwnerGuid = _ownerPerson.PersonGuid, IsDeleted = false });
            friends.Add(new Friend() { FriendGuid = friendBeate.PersonGuid, OwnerGuid = _ownerPerson.PersonGuid, IsDeleted = false });
            friendQueryMock.Setup(mock => mock.GetFriends(_ownerPerson.PersonGuid)).Returns(friends);

            var friendCommandMock = _kernel.GetMock<IFriendrequestCommand>();
            friendCommandMock.Setup(mock => mock.SaveFriendrequest(It.IsAny<SaveFriendrequestModel>()));

            var friendrequestQueryMock = _kernel.GetMock<IFriendrequestQuery>();
            friendrequestQueryMock.Setup(mock => mock.GetFriendrequestsByRequester(It.IsAny<Guid>())).Returns(new List<Friendrequest>());

            var friendrequestRepository = _kernel.Get<IFriendrequestRepository>();
            friendrequestRepository.SendFriendrequest(username, friendUsername);

            friendQueryMock.VerifyAll();
            personQueryMock.VerifyAll();
            friendCommandMock.VerifyAll();
            friendrequestQueryMock.VerifyAll();
        }  
        
        
        [Test]
        public void When_sending_a_friendrequest__it_should_fail()
        {

            // Fail because the friend is already a Friend

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
            const string friendUsername = "beate@wizz.com";

            var personQueryMock = _kernel.GetMock<IPersonQuery>();
            personQueryMock.Setup(mock => mock.GetPersonByUsername(username)).Returns(_ownerPerson);
            personQueryMock.Setup(mock => mock.GetPersonByUsername(friendUsername)).Returns(friendBeate);

            var friendQueryMock = _kernel.GetMock<IFriendQuery>();
            List<Friend> friends = new List<Friend>();
            friends.Add(new Friend() { FriendGuid = friendMarianne.PersonGuid, OwnerGuid = _ownerPerson.PersonGuid, IsDeleted = false });
            friends.Add(new Friend() { FriendGuid = friendBeate.PersonGuid, OwnerGuid = _ownerPerson.PersonGuid, IsDeleted = false });
            friendQueryMock.Setup(mock => mock.GetFriends(_ownerPerson.PersonGuid)).Returns(friends);

            var friendrequestQueryMock = _kernel.GetMock<IFriendrequestQuery>();
            friendrequestQueryMock.Setup(mock => mock.GetFriendrequestsByRequester(It.IsAny<Guid>())).Returns(new List<Friendrequest>());

            var friendCommandMock = _kernel.GetMock<IFriendrequestCommand>();
            friendCommandMock.Setup(mock => mock.SaveFriendrequest(It.IsAny<SaveFriendrequestModel>()));

            var friendrequestRepository = _kernel.Get<IFriendrequestRepository>();
            Action action = () =>
            {
                friendrequestRepository.SendFriendrequest(username, friendUsername);
            };

            action.ShouldThrow<Domain.Friendrequest.FriendrequestRepository.FriendrequestRepositoryException>();

            friendQueryMock.VerifyAll();
            personQueryMock.VerifyAll();
            friendCommandMock.Verify(a => a.SaveFriendrequest(It.IsAny<SaveFriendrequestModel>()), Times.Never());
        }

        [Test]
        public void When_sending_a_friendrequest__it_should_fail2()
        {

            // Fail because you cannot befriend yourself

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
            const string friendUsername = "beate@wizz.com";

            var personQueryMock = _kernel.GetMock<IPersonQuery>();
            personQueryMock.Setup(mock => mock.GetPersonByUsername(username)).Returns(_ownerPerson);


            var friendQueryMock = _kernel.GetMock<IFriendQuery>();
            List<Friend> friends = new List<Friend>();
            friends.Add(new Friend() { FriendGuid = friendMarianne.PersonGuid, OwnerGuid = _ownerPerson.PersonGuid, IsDeleted = false });
            friends.Add(new Friend() { FriendGuid = friendBeate.PersonGuid, OwnerGuid = _ownerPerson.PersonGuid, IsDeleted = false });
            friendQueryMock.Setup(mock => mock.GetFriends(_ownerPerson.PersonGuid)).Returns(friends);

            var friendCommandMock = _kernel.GetMock<IFriendrequestCommand>();
            friendCommandMock.Setup(mock => mock.SaveFriendrequest(It.IsAny<SaveFriendrequestModel>()));

            var friendrequestQueryMock = _kernel.GetMock<IFriendrequestQuery>();
            friendrequestQueryMock.Setup(mock => mock.GetFriendrequestsByRequester(It.IsAny<Guid>())).Returns(new List<Friendrequest>());

            var friendrequestRepository = _kernel.Get<IFriendrequestRepository>();
            Action action = () =>
            {
                friendrequestRepository.SendFriendrequest(username, username); // ** send to yourself
            };

            action.ShouldThrow<Domain.Friendrequest.FriendrequestRepository.FriendrequestRepositoryException>();

            friendQueryMock.VerifyAll();
            personQueryMock.VerifyAll();
            friendCommandMock.Verify(a => a.SaveFriendrequest(It.IsAny<SaveFriendrequestModel>()), Times.Never());
        }

        [Test]
        public void When_sending_a_friendrequest__it_should_fail3()
        {

            // Fail because you already have sent a friendrequest to that peson

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
            const string friendUsername = "beate@wizz.com";

            var personQueryMock = _kernel.GetMock<IPersonQuery>();
            personQueryMock.Setup(mock => mock.GetPersonByUsername(username)).Returns(_ownerPerson);
            personQueryMock.Setup(mock => mock.GetPersonByUsername(friendUsername)).Returns(friendBeate);

            var friendQueryMock = _kernel.GetMock<IFriendQuery>();
            List<Friend> friends = new List<Friend>();
            friends.Add(new Friend() { FriendGuid = friendMarianne.PersonGuid, OwnerGuid = _ownerPerson.PersonGuid, IsDeleted = false });
            friends.Add(new Friend() { FriendGuid = friendBeate.PersonGuid, OwnerGuid = _ownerPerson.PersonGuid, IsDeleted = false });
            friendQueryMock.Setup(mock => mock.GetFriends(_ownerPerson.PersonGuid)).Returns(friends);

            var friendCommandMock = _kernel.GetMock<IFriendrequestCommand>();
            friendCommandMock.Setup(mock => mock.SaveFriendrequest(It.IsAny<SaveFriendrequestModel>()));

            var friendrequestQueryMock = _kernel.GetMock<IFriendrequestQuery>();
            friendrequestQueryMock.Setup(mock => mock.GetFriendrequestsByRequester(It.IsAny<Guid>())).Returns(new List<Friendrequest>(){new Friendrequest()
            {
                PersonRequestedGuid = friendBeate.PersonGuid,
                RequesterPersonGuid = _ownerPerson.PersonGuid
            }});

            var friendrequestRepository = _kernel.Get<IFriendrequestRepository>();
            Action action = () =>
            {
                friendrequestRepository.SendFriendrequest(username, friendUsername);
            };

            action.ShouldThrow<Domain.Friendrequest.FriendrequestRepository.FriendrequestRepositoryException>();

            friendQueryMock.VerifyAll();
            personQueryMock.VerifyAll();
            friendCommandMock.Verify(a => a.SaveFriendrequest(It.IsAny<SaveFriendrequestModel>()), Times.Never());
        }

        [Test]
        public void When_sending_a_friendrequest__it_should_succeed2()
        {
            /*
             
                Action: Send friendrequest

                Before:
                Geir logger inn på mobil(id:1)
                Marianne logger inn på mobil(id:2)
                Geir sender friendrequest til Marianne
                Marianne sender friendrequest til Geir
                
                After:
                Automatisk lag nye Friends fordi det finnes allerede en Friendrequest fra Geir

            */

            var friendMarianne = new Person()
            {
                PersonGuid = new Guid("14DD5F5A-4747-4747-4747-64A821B1EAA0"),
                Displayname = "Marianne",
                IsDeleted = false
            };


            const string username = "smurf@smurf.com";
            const string friendUsername = "marianne@woot.com";

            var personQueryMock = _kernel.GetMock<IPersonQuery>();
            personQueryMock.Setup(mock => mock.GetPersonByUsername(username)).Returns(_ownerPerson);
            personQueryMock.Setup(mock => mock.GetPersonByUsername(friendUsername)).Returns(friendMarianne);


            var friendQueryMock = _kernel.GetMock<IFriendQuery>();
            List<Friend> friends = new List<Friend>();
            friendQueryMock.Setup(mock => mock.GetFriends(_ownerPerson.PersonGuid)).Returns(friends);

            var friendrequestQueryMock = _kernel.GetMock<IFriendrequestQuery>();
            friendrequestQueryMock.Setup(mock => mock.GetFriendrequestsByRequester(_ownerPerson.PersonGuid)).Returns(new List<Friendrequest>());

            var friendRequestsMarianne = new List<Friendrequest>();
            friendRequestsMarianne.Add(new Friendrequest()
            {
                PersonRequestedGuid = _ownerPerson.PersonGuid,
                RequesterPersonGuid = friendMarianne.PersonGuid
            });
            friendrequestQueryMock.Setup(mock => mock.GetFriendrequestsByRequester(friendMarianne.PersonGuid)).Returns(friendRequestsMarianne);

            var friendCommandMock = _kernel.GetMock<IFriendrequestCommand>();
            friendCommandMock.Setup(mock => mock.SaveFriendrequest(It.IsAny<SaveFriendrequestModel>()));

            var acceptFriendrequestLogicMock = _kernel.GetMock<IAcceptFriendrequestLogic>();
            acceptFriendrequestLogicMock.Setup(mock => mock.AcceptFriendrequest(username, friendUsername));

            var friendrequestRepository = _kernel.Get<IFriendrequestRepository>();
            friendrequestRepository.SendFriendrequest(username, friendUsername);

            friendCommandMock.Verify(a => a.SaveFriendrequest(It.IsAny<SaveFriendrequestModel>()), Times.Never());

            friendQueryMock.VerifyAll();
            personQueryMock.VerifyAll();
            friendrequestQueryMock.VerifyAll();
            acceptFriendrequestLogicMock.VerifyAll();
        }



        [Test]
        public void When_sending_a_friendrequest__it_should_succeed3()
        {
            /*
                Action: Send friendrequest

                Before:
                Geir logger inn på mobil(id:1)
                Marianne logger inn på mobil(id:2)
                Geir sender friendrequest til Marianne
                Marianne accepts friendrequest
                Geir sletter friendrequest til Marianne, og synkroniserer
                Marianne synkroniserer
                Geir sender nytt Friendrequest til Marianne
                
                After:
                Det er lov til å sende nytt Friendrequest. Marianne får da ny mulighet til å bli venn med Geir.
            */

            var friendMarianne = new Person()
            {
                PersonGuid = new Guid("14DD5F5A-4747-4747-4747-64A821B1EAA0"),
                Displayname = "Marianne",
                IsDeleted = false
            };


            const string username = "smurf@smurf.com";
            const string friendUsername = "marianne@woot.com";

            var personQueryMock = _kernel.GetMock<IPersonQuery>();
            personQueryMock.Setup(mock => mock.GetPersonByUsername(username)).Returns(_ownerPerson);
            personQueryMock.Setup(mock => mock.GetPersonByUsername(friendUsername)).Returns(friendMarianne);


            var friendQueryMock = _kernel.GetMock<IFriendQuery>();
            List<Friend> friends = new List<Friend>();
            friendQueryMock.Setup(mock => mock.GetFriends(_ownerPerson.PersonGuid)).Returns(friends); // *** Returns only Friends that are not Deleted == True ***

            var friendrequestQueryMock = _kernel.GetMock<IFriendrequestQuery>();
            friendrequestQueryMock.Setup(mock => mock.GetFriendrequestsByRequester(_ownerPerson.PersonGuid)).Returns(new List<Friendrequest>());

            var friendRequestsMarianne = new List<Friendrequest>();
            friendRequestsMarianne.Add(new Friendrequest()
            {
                PersonRequestedGuid = _ownerPerson.PersonGuid,
                RequesterPersonGuid = friendMarianne.PersonGuid
            });
            friendrequestQueryMock.Setup(mock => mock.GetFriendrequestsByRequester(friendMarianne.PersonGuid)).Returns(friendRequestsMarianne);

            var friendCommandMock = _kernel.GetMock<IFriendrequestCommand>();
            friendCommandMock.Setup(mock => mock.SaveFriendrequest(It.IsAny<SaveFriendrequestModel>()));

            var acceptFriendrequestLogicMock = _kernel.GetMock<IAcceptFriendrequestLogic>();
            acceptFriendrequestLogicMock.Setup(mock => mock.AcceptFriendrequest(username, friendUsername));

            var friendrequestRepository = _kernel.Get<IFriendrequestRepository>();
            friendrequestRepository.SendFriendrequest(username, friendUsername);

            friendCommandMock.Verify(a => a.SaveFriendrequest(It.IsAny<SaveFriendrequestModel>()), Times.Never());

            friendQueryMock.VerifyAll();
            personQueryMock.VerifyAll();
            friendrequestQueryMock.VerifyAll();
            acceptFriendrequestLogicMock.VerifyAll();
        }  


    }
}

