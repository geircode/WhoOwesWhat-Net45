//using System;
//using FluentAssertions;
//using Moq;
//using Ninject;
//using Ninject.MockingKernel.Moq;
//using NUnit.Framework;
//using WhoOwesWhat.DataProvider.Interfaces;
//using WhoOwesWhat.Domain;
//using WhoOwesWhat.Domain.DTO;
//using WhoOwesWhat.Domain.Friend;
//using WhoOwesWhat.Domain.Interfaces;
//// ReSharper disable once ConditionIsAlwaysTrueOrFalse

//namespace WhoOwesWhat.Tests.FriendRepository
//{

//    [TestFixture]
//    public class SyncOfflineFriendTests
//    {
//        private static MoqMockingKernel _kernel;
//        Person _ownerPerson;

//        public SyncOfflineFriendTests()
//        {
//            _kernel = new MoqMockingKernel();
//            _kernel.Bind<IFriendRepository>().To<Domain.Friend.FriendRepository>();
//        }

//        [SetUp]
//        public void SetUp()
//        {
//            _kernel.Reset();

//            _ownerPerson = new Person()
//            {
//                PersonGuid = new Guid("FAD0CC47-1337-1337-1337-0B34360C33FA"),
//                Displayname = "Garg1337",
//                IsDeleted = false
//            };
//        }

//        [Test]
//        public void When_sync_for_the_first_time()
//        {

//            /*
//                Geir legger til Marianne som OfflineFriend på App
//                Geir synkroniserer              
//             */


//            Guid personGuid = new Guid("14DD5F5A-4747-4747-4747-64A821B1EAA0");
//            const string displayname = "Marianne";
//            const bool isPersonDeleted = false;
//            const bool isFriendDeleted = false;

//            var friendPerson = new Person()
//            {
//                PersonGuid = personGuid,
//                Displayname = displayname,
//                IsDeleted = false
//            };
//            Person friendPersonCallback = null;

//            const string username = "smurf@smurf.com";

//            var personQueryMock = _kernel.GetMock<IPersonQuery>();
//            personQueryMock.Setup(mock => mock.GetPersonByUsername(username)).Returns(_ownerPerson);
//            personQueryMock.Setup(mock => mock.GetPerson(personGuid)).Returns((Person)null); // ** it's null **

//            var personCommandMock = _kernel.GetMock<IPersonCommand>();
//            personCommandMock.Setup(mock => mock.SavePerson(It.IsAny<Person>()))
//                .Callback<Person>((obj) => friendPersonCallback = obj);

//            var friendCommandMock = _kernel.GetMock<IFriendCommand>();
//            Friend friendCallback = null;
//            friendCommandMock.Setup(mock => mock.SaveFriend(It.IsAny<Friend>()))
//                .Callback<Friend>((obj) => friendCallback = obj);
//                //.Returns(new void() { IsSuccess = true });

//            var friendRepositoryLogicMock = _kernel.GetMock<IFriendRepositoryLogic>();
//            var newFriend = new Friend()
//            {
//                FriendGuid = personGuid,
//                OwnerGuid = _ownerPerson.PersonGuid,
//                IsDeleted = isFriendDeleted
//            };
//            friendRepositoryLogicMock.Setup(mock => mock.MapToFriend(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>())).Returns(newFriend);

//            var friendQueryMock = _kernel.GetMock<IFriendQuery>();
//            friendQueryMock.Setup(mock => mock.GetFriend(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns((Friend)null);

//            var credentialQueryMock = _kernel.GetMock<IUserCredentialQuery>();
//            credentialQueryMock.Setup(mock => mock.IsNotOnlinePerson(It.IsAny<Guid>())).Returns(true);

//            var friendRepository = _kernel.Get<IFriendRepository>();

//            bool isUsedInPosts = false;

//            friendRepository.SyncOfflineFriend(username, personGuid, displayname, isUsedInPosts, isFriendDeleted);

//            personQueryMock.VerifyAll();
//            friendCommandMock.VerifyAll();
//            personCommandMock.VerifyAll();

//            friendPersonCallback.ShouldBeEquivalentTo(friendPerson);

//            personQueryMock.Verify(a => a.GetPerson(It.IsAny<Guid>()), Times.Once);
//            personCommandMock.Verify(a => a.SavePerson(It.IsAny<Person>()), Times.Once());

//            friendCallback.ShouldBeEquivalentTo(newFriend);
//        }

//        [Test]
//        public void When_sync_for_the_second_time()
//        {

//            /*
//             Action: Synchronize OfflineFriends from App (only OfflinePersons can be added directly on the app)
             
//                Geir legger til Marianne som OfflineFriend på App
//                Geir synkroniserer              
//                Geir synkroniserer for the second time    
             
//             After:
//                MMF: I steden for å introdusere versjonshåndtering og dirty flags så synkroniserer vi alt av ** Persons og Friends ** hver eneste gang
                
//                En Friend blir ikke oppdatert med mindre den er slettet
//             */

//            Guid personGuid = new Guid("14DD5F5A-4747-4747-4747-64A821B1EAA0");
//            const string displayname = "Marianne";
//            const bool isPersonDeleted = false;
//            const bool isFriendDeleted = false;

//            var friendPerson = new Person()
//            {
//                PersonGuid = personGuid,
//                Displayname = displayname,
//                IsDeleted = false
//            };

//            const string username = "smurf@smurf.com";

//            var personCommandMock = _kernel.GetMock<IPersonCommand>();

//            var personQueryMock = _kernel.GetMock<IPersonQuery>();
//            personQueryMock.Setup(mock => mock.GetPersonByUsername(username)).Returns(_ownerPerson);
            
//            var friendCommandMock = _kernel.GetMock<IFriendCommand>();


//            var friendQueryMock = _kernel.GetMock<IFriendQuery>();
//            var existingFriend = new Friend() {FriendGuid = friendPerson.PersonGuid, OwnerGuid = _ownerPerson.PersonGuid, IsDeleted = false};
//            friendQueryMock.Setup(mock => mock.GetFriend(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(existingFriend);

//            var friendRepositoryLogicMock = _kernel.GetMock<IFriendRepositoryLogic>();
//            existingFriend.IsDeleted = friendPerson.IsDeleted;
//            friendRepositoryLogicMock.Setup(a => a.MapToFriend(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>())).Returns(existingFriend);

//            var credentialQueryMock = _kernel.GetMock<IUserCredentialQuery>();
//            credentialQueryMock.Setup(mock => mock.IsNotOnlinePerson(It.IsAny<Guid>())).Returns(true);

//            var friendRepository = _kernel.Get<IFriendRepository>();

//            friendRepository.SyncOfflineFriend(username, personGuid, displayname, false, isFriendDeleted);

//            personQueryMock.VerifyAll();
//            credentialQueryMock.VerifyAll();
//            friendQueryMock.VerifyAll();

//            personQueryMock.Verify(a => a.GetPerson(It.IsAny<Guid>()), Times.Never());
//            personCommandMock.Verify(a => a.SavePerson(It.IsAny<Person>()), Times.Never());
//            friendCommandMock.Verify(a => a.SaveFriend(It.IsAny<Friend>()), Times.Never());
//            friendRepositoryLogicMock.Verify(a => a.MapToFriend(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>()), Times.Never());
//        }

//        [Test]
//        public void When_sync_after_deleting_OfflineFriend_on_App_1()
//        {
//            /*
//             Action: Delete Friend Marianne

//             Before:
//                Geir legger til Marianne som OfflineFriend på App
//                Geir synkroniserer  
//                Geir sletter Marianne på App (Marianne er ikke i bruk i App)
//                Geir synkroniserer
             
//             After:
//             Friend(Marianne) and Person(Marianne) is marked IsDeleted = True on the server

//             */

//            Guid personGuid = new Guid("14DD5F5A-4747-4747-4747-64A821B1EAA0");
//            const string displayname = "Marianne";
//            const bool isFriendDeleted = true; // *** TRUE ***

//            var friendPerson = new Person()
//            {
//                PersonGuid = personGuid,
//                Displayname = displayname,
//                IsDeleted = false
//            };

//            const string username = "smurf@smurf.com";

//            var credentialQueryMock = _kernel.GetMock<IUserCredentialQuery>();
//            credentialQueryMock.Setup(mock => mock.IsNotOnlinePerson(It.IsAny<Guid>())).Returns(true);

//            var personCommandMock = _kernel.GetMock<IPersonCommand>();

//            var personQueryMock = _kernel.GetMock<IPersonQuery>();
//            personQueryMock.Setup(mock => mock.GetPersonByUsername(username)).Returns(_ownerPerson);

//            var friendCommandMock = _kernel.GetMock<IFriendCommand>();

//            var friendQueryMock = _kernel.GetMock<IFriendQuery>();
//            var existingFriend = new Friend() { FriendGuid = friendPerson.PersonGuid, OwnerGuid = _ownerPerson.PersonGuid, IsDeleted = false }; // *** FALSE ***
//            friendQueryMock.Setup(mock => mock.GetFriend(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(existingFriend);

//            var friendRepositoryLogicMock = _kernel.GetMock<IFriendRepositoryLogic>();

//            var transactionQueryMock = _kernel.GetMock<ITransactionQuery>();
//            transactionQueryMock.Setup(mock => mock.IsPersonUsedInAnyPosts(It.IsAny<Guid>())).Returns(false);

//            var deleteOfflineFriendLogicMock = _kernel.GetMock<IDeleteFriendCommand>();
//            deleteOfflineFriendLogicMock.Setup(mock => mock.DeleteOfflineFriend(It.IsAny<string>(), It.IsAny<Guid>()));


//            var friendRepository = _kernel.Get<IFriendRepository>();
//            friendRepository.SyncOfflineFriend(username, personGuid, displayname, false, isFriendDeleted);

//            personCommandMock.VerifyAll();
//            personQueryMock.VerifyAll();
//            friendCommandMock.VerifyAll();
//            credentialQueryMock.VerifyAll();
//            friendQueryMock.VerifyAll();
//            friendRepositoryLogicMock.VerifyAll();
//            deleteOfflineFriendLogicMock.VerifyAll();
//            transactionQueryMock.VerifyAll();

//            personQueryMock.Verify(a => a.GetPerson(It.IsAny<Guid>()), Times.Never());
//            personCommandMock.Verify(a => a.SavePerson(It.IsAny<Person>()), Times.Never());
//            friendRepositoryLogicMock.Verify(a => a.MapToFriend(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>()), Times.Never);
//            friendCommandMock.Verify(a => a.SaveFriend(It.IsAny<Friend>()), Times.Never);

//        }


//        [Test]
//        public void When_sync_after_deleting_OfflineFriend_on_App_2()
//        {
//            /*
//             Action: Delete Friend Marianne

//             Before:
//                Geir legger til Marianne som OfflineFriend på Mobil(id=*1*)
//                Geir synkroniserer Mobil(id=*1*)
//                Geir logger inn på Mobil(id=*2*) og synkroniserer
//                Geir legger til Marianne som Payer på en POST på Mobil(id=2)
//                Geir synkroniserer Mobil(id=*2*)
//                Geir sletter Marianne på Mobil(id=*1*)
//                Geir synkroniserer Mobil(id=*1*)
             
//             After:
//             Siden Marianne er i bruk på server så har server presedens over mobil(id=1) og legger Marianne inn som Friend igjen på Mobil(id=*1*)
//             Synkronisering til Server ignorerer sletting av Marianne.

//             */

//            Guid personGuid = new Guid("14DD5F5A-4747-4747-4747-64A821B1EAA0");
//            const string displayname = "Marianne";
//            const bool isFriendDeleted = true; // *** TRUE ***

//            var friendPerson = new Person()
//            {
//                PersonGuid = personGuid,
//                Displayname = displayname,
//                IsDeleted = false
//            };

//            const string username = "smurf@smurf.com";

//            var credentialQueryMock = _kernel.GetMock<IUserCredentialQuery>();
//            credentialQueryMock.Setup(mock => mock.IsNotOnlinePerson(It.IsAny<Guid>())).Returns(true);

//            var personCommandMock = _kernel.GetMock<IPersonCommand>();

//            var personQueryMock = _kernel.GetMock<IPersonQuery>();
//            personQueryMock.Setup(mock => mock.GetPersonByUsername(username)).Returns(_ownerPerson);

//            var friendCommandMock = _kernel.GetMock<IFriendCommand>();

//            var friendQueryMock = _kernel.GetMock<IFriendQuery>();
//            var existingFriend = new Friend() { FriendGuid = friendPerson.PersonGuid, OwnerGuid = _ownerPerson.PersonGuid, IsDeleted = false }; // *** FALSE ***
//            friendQueryMock.Setup(mock => mock.GetFriend(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(existingFriend);

//            var friendRepositoryLogicMock = _kernel.GetMock<IFriendRepositoryLogic>();

//            var transactionQueryMock = _kernel.GetMock<ITransactionQuery>();
//            transactionQueryMock.Setup(mock => mock.IsPersonUsedInAnyPosts(It.IsAny<Guid>())).Returns(true); // *** TRUE ***

//            var deleteOfflineFriendLogicMock = _kernel.GetMock<IDeleteFriendCommand>();
//            deleteOfflineFriendLogicMock.Setup(mock => mock.DeleteOfflineFriend(It.IsAny<string>(), It.IsAny<Guid>()));


//            var friendRepository = _kernel.Get<IFriendRepository>();
//            friendRepository.SyncOfflineFriend(username, personGuid, displayname, false, isFriendDeleted);

//            personCommandMock.VerifyAll();
//            personQueryMock.VerifyAll();
//            friendCommandMock.VerifyAll();
//            credentialQueryMock.VerifyAll();
//            friendQueryMock.VerifyAll();
//            friendRepositoryLogicMock.VerifyAll();
//            transactionQueryMock.VerifyAll();

//            personQueryMock.Verify(a => a.GetPerson(It.IsAny<Guid>()), Times.Never());
//            personCommandMock.Verify(a => a.SavePerson(It.IsAny<Person>()), Times.Never());
//            friendRepositoryLogicMock.Verify(a => a.MapToFriend(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>()), Times.Never);
//            friendCommandMock.Verify(a => a.SaveFriend(It.IsAny<Friend>()), Times.Never);
//            deleteOfflineFriendLogicMock.Verify(a => a.DeleteOfflineFriend(It.IsAny<string>(), It.IsAny<Guid>()), Times.Never);
//        }


//        //[Test]
//        //public void When_sync_after_deleting_OfflineFriend_on_App_3()
//        //{
//        //    /*
//        //     Action: Delete Friend Marianne

//        //     Before:
//        //        Geir legger til Marianne som OfflineFriend på Mobil(id=*1*)
//        //        Geir synkroniserer Mobil(id=*1*)
//        //        Geir logger inn på Mobil(id=*2*) og synkroniserer
//        //     *  Geir sletter Marianne på Mobil(id=*1*)
//        //        Geir synkroniserer Mobil(id=*1*)
//        //        Geir legger til Marianne som Payer på en POST på Mobil(id=2)
//        //        Geir synkroniserer Mobil(id=*2*)
   
             
//        //     After:
//        //     Siden Marianne er i bruk på Mobil(id=2), så har dette presedens over slettingen på server og legger Marianne inn som OfflineFriend igjen på Server
//        //     */

//        //    Guid personGuid = new Guid("14DD5F5A-4747-4747-4747-64A821B1EAA0");
//        //    const string displayname = "Marianne";
//        //    const bool isFriendDeleted = false; // *** FALSE ***

//        //    var friendPerson = new Person()
//        //    {
//        //        PersonGuid = personGuid,
//        //        Displayname = displayname,
//        //        IsDeleted = false
//        //    };

//        //    const string username = "smurf@smurf.com";

//        //    var credentialQueryMock = _kernel.GetMock<IUserCredentialQuery>();
//        //    credentialQueryMock.Setup(mock => mock.IsNotOnlinePerson(It.IsAny<Guid>())).Returns(true);

//        //    var personCommandMock = _kernel.GetMock<IPersonCommand>();

//        //    var personQueryMock = _kernel.GetMock<IPersonQuery>();
//        //    personQueryMock.Setup(mock => mock.GetPersonByUsername(username)).Returns(_ownerPerson);

//        //    var friendCommandMock = _kernel.GetMock<IFriendCommand>();


//        //    var friendQueryMock = _kernel.GetMock<IFriendQuery>();
//        //    var existingFriend = new Friend() { FriendGuid = friendPerson.PersonGuid, OwnerGuid = _ownerPerson.PersonGuid, IsDeleted = true }; // *** TRUE ***
//        //    friendQueryMock.Setup(mock => mock.GetFriend(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(existingFriend);

//        //    var friendRepositoryLogicMock = _kernel.GetMock<IFriendRepositoryLogic>();

//        //    var transactionQueryMock = _kernel.GetMock<ITransactionQuery>();
//        //    transactionQueryMock.Setup(mock => mock.IsPersonUsedInAnyPosts(It.IsAny<Guid>())).Returns(false);

//        //    var deleteOfflineFriendLogicMock = _kernel.GetMock<IDeleteFriendCommand>();
//        //    deleteOfflineFriendLogicMock.Setup(mock => mock.UndeleteOfflineFriend(friendPerson.PersonGuid, _ownerPerson.PersonGuid));


//        //    var friendRepository = _kernel.Get<IFriendRepository>();
//        //    const bool isInUse = true; // meaning the the friend is in use on the App
//        //    friendRepository.SyncOfflineFriend(username, personGuid, displayname, isInUse, isFriendDeleted);

//        //    personCommandMock.VerifyAll();
//        //    personQueryMock.VerifyAll();
//        //    friendCommandMock.VerifyAll();
//        //    credentialQueryMock.VerifyAll();
//        //    friendQueryMock.VerifyAll();
//        //    friendRepositoryLogicMock.VerifyAll();

//        //    personQueryMock.Verify(a => a.GetPerson(It.IsAny<Guid>()), Times.Never());
//        //    personCommandMock.Verify(a => a.SavePerson(It.IsAny<Person>()), Times.Never());
//        //    friendRepositoryLogicMock.Verify(a => a.MapToFriend(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>()), Times.Never);
//        //    friendCommandMock.Verify(a => a.SaveFriend(It.IsAny<Friend>()), Times.Never);
//        //    deleteOfflineFriendLogicMock.Verify(a => a.DeleteOfflineFriend(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
//        //    transactionQueryMock.Verify(a => a.IsPersonUsedInAnyPosts(It.IsAny<Guid>()), Times.Never);
//        //}


//    }
//}

