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
//    public class SyncOnlineFriendTests
//    {
//        private static MoqMockingKernel _kernel;
//        Person _ownerPerson;
//        private UserCredential _ownerCredential;

//        public SyncOnlineFriendTests()
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
//            _ownerCredential = new UserCredential()
//            {
//                Email = "smurf@smurf.com",
//                Username = "smurf@smurf.com",
//                Person = _ownerPerson
//            };
//        }

//        //[Test]
//        //public void When_sync_for_the_first_time()
//        //{
//        //    /*
//        //     Action: No action, because noone is deleted

//        //     Before:
//        //        Geir sender Friendrequest til Marianne
//        //        Marianne accepts Friendrequest
//        //        Geir synkroniserer
             
//        //     After:
//        //     Ingen update
//        //     */

//        //    // DeleteOrUndeleteOnlineFriend parameteres:
//        //    string ownerUsername = _ownerCredential.Username;
//        //    Guid marianneGuid = new Guid("14DD5F5A-4747-4747-4747-64A821B1EAA0");
//        //    const string displayname = "Marianne";
//        //    const bool isFriendDeleted = false;

//        //    var personQueryMock = _kernel.GetMock<IPersonQuery>();
//        //    personQueryMock.Setup(mock => mock.GetPersonByUsername(ownerUsername)).Returns(_ownerPerson);

//        //    var credentialQueryMock = _kernel.GetMock<IUserCredentialQuery>();
//        //    credentialQueryMock.Setup(mock => mock.IsOnlinePerson(marianneGuid)).Returns(true);

//        //    var friendQueryMock = _kernel.GetMock<IFriendQuery>();
//        //    var marianneFriend = new Friend() { FriendGuid = marianneGuid, OwnerGuid = _ownerPerson.PersonGuid, IsDeleted = false };
//        //    friendQueryMock.Setup(mock => mock.GetFriend(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(marianneFriend);

//        //    var transactionQueryMock = _kernel.GetMock<ITransactionQuery>();
//        //    transactionQueryMock.Setup(mock => mock.IsPersonUsedInAnyPosts(It.IsAny<Guid>())).Returns(false);

//        //    var friendRepository = _kernel.Get<IFriendRepository>();

//        //    const bool isUsedInPosts = false;
         
//        //    friendRepository.DeleteOrUndeleteOnlineFriend(ownerUsername, marianneGuid, displayname, isUsedInPosts, isFriendDeleted);

//        //    personQueryMock.VerifyAll();
//        //    credentialQueryMock.VerifyAll();
//        //    friendQueryMock.VerifyAll();

//        //    transactionQueryMock.Verify(mock => mock.IsPersonUsedInAnyPosts(It.IsAny<Guid>()), Times.Never);
//        //}


//        [Test]
//        public void When_sync_after_deleting_OfflineFriend_on_App_1()
//        {
//            /*
//             Action: Delete Friend Marianne

//             Before:
//                Geir sender Friendrequest til Marianne
//                Marianne accepts Friendrequest
//                Geir synkroniserer
//                Geir sletter Marianne på App (Marianne er ikke i bruk i App)
//                Geir synkroniserer
             
//             After:
//             Friend(Marianne) to Owner(Geir) is marked IsDeleted = True on the server
//             Friend(Geir) to Owner(Marianne) is marked IsDeleted = True on the server

//             */


//            // DeleteOrUndeleteOnlineFriend parameteres:
//            string ownerUsername = _ownerCredential.Username;
//            Guid marianneGuid = new Guid("14DD5F5A-4747-4747-4747-64A821B1EAA0");
//            const string displayname = "Marianne";
//            const bool isFriendDeleted = true; // ** TRUE **

//            var credentialQueryMock = _kernel.GetMock<IUserCredentialQuery>();
//            credentialQueryMock.Setup(mock => mock.IsOnlinePerson(It.IsAny<Guid>())).Returns(true);

//            var personQueryMock = _kernel.GetMock<IPersonQuery>();
//            personQueryMock.Setup(mock => mock.GetPersonByUsername(ownerUsername)).Returns(_ownerPerson);

//            var friendQueryMock = _kernel.GetMock<IFriendQuery>();
//            var existingFriend = new Friend() { FriendGuid = marianneGuid, OwnerGuid = _ownerPerson.PersonGuid, IsDeleted = false }; // *** FALSE ***
//            friendQueryMock.Setup(mock => mock.GetFriend(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(existingFriend);

//            var transactionQueryMock = _kernel.GetMock<ITransactionQuery>();
//            transactionQueryMock.Setup(mock => mock.IsPersonUsedInAnyPosts(It.IsAny<Guid>())).Returns(false);

//            var deleteFriendLogicMock = _kernel.GetMock<IDeleteFriendCommand>();
//            deleteFriendLogicMock.Setup(mock => mock.DeleteOnlineFriend(It.IsAny<Guid>(), It.IsAny<Guid>()));

//            var friendRepository = _kernel.Get<IFriendRepository>();
//            friendRepository.DeleteOrUndeleteOnlineFriend(ownerUsername, marianneGuid, displayname, false, isFriendDeleted);

//            personQueryMock.VerifyAll();
//            credentialQueryMock.VerifyAll();
//            friendQueryMock.VerifyAll();
//            deleteFriendLogicMock.VerifyAll();
//            transactionQueryMock.VerifyAll();

//        }


//        [Test]
//        public void When_sync_after_deleting_OfflineFriend_on_App_2()
//        {
//            /*
//             Action: Delete Friend Marianne

//             Before:
//                Geir sender friendrequest til Marianne på Mobil(id=*1*)
//                Marianne accepts friendrequest
//                Geir synkroniserer Mobil(id=*1*) og får Marianne som OnlineFriend
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
//            credentialQueryMock.Setup(mock => mock.IsOnlinePerson(It.IsAny<Guid>())).Returns(true);

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

//            var friendRepository = _kernel.Get<IFriendRepository>();
//            friendRepository.DeleteOrUndeleteOnlineFriend(username, personGuid, displayname, false, isFriendDeleted);

//            personCommandMock.VerifyAll();
//            personQueryMock.VerifyAll();
//            friendCommandMock.VerifyAll();
//            credentialQueryMock.VerifyAll();
//            friendQueryMock.VerifyAll();
//            friendRepositoryLogicMock.VerifyAll();
//            transactionQueryMock.VerifyAll();
//            deleteOfflineFriendLogicMock.VerifyAll();

//            personQueryMock.Verify(a => a.GetPerson(It.IsAny<Guid>()), Times.Never());
//            personCommandMock.Verify(a => a.SavePerson(It.IsAny<Person>()), Times.Never());
//            friendRepositoryLogicMock.Verify(a => a.MapToFriend(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>()), Times.Never);
//            friendCommandMock.Verify(a => a.SaveFriend(It.IsAny<Friend>()), Times.Never);
//            deleteOfflineFriendLogicMock.Verify(a => a.DeleteOfflineFriend(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
//        }


//        [Test]
//        public void When_sync_after_deleting_OnlineFriend_on_App_3()
//        {
//            /*
//             Action: Delete Friend Marianne

//             Before:
//                Geir legger til Marianne som OnlineFriend på Mobil(id=*1*)
//                Geir synkroniserer Mobil(id=*1*)
//                Geir logger inn på Mobil(id=*2*) og synkroniserer
//             *  Geir sletter Marianne på Mobil(id=*1*)
//                Geir synkroniserer Mobil(id=*1*)
//                Geir legger til Marianne som Payer på en POST på Mobil(id=2)
//                Geir synkroniserer Mobil(id=*2*)
   
             
//             After:
//             Siden Marianne er i bruk på Mobil(id=2), så har dette presedens over slettingen på server og legger Marianne inn som OfflineFriend igjen på Server
//             */

//            Guid personGuid = new Guid("14DD5F5A-4747-4747-4747-64A821B1EAA0");
//            const string displayname = "Marianne";
//            const bool isFriendDeleted = false; // *** FALSE ***

//            var friendPerson = new Person()
//            {
//                PersonGuid = personGuid,
//                Displayname = displayname,
//                IsDeleted = false
//            };

//            const string username = "smurf@smurf.com";

//            var credentialQueryMock = _kernel.GetMock<IUserCredentialQuery>();
//            credentialQueryMock.Setup(mock => mock.IsOnlinePerson(It.IsAny<Guid>())).Returns(true);

//            var personCommandMock = _kernel.GetMock<IPersonCommand>();

//            var personQueryMock = _kernel.GetMock<IPersonQuery>();
//            personQueryMock.Setup(mock => mock.GetPersonByUsername(username)).Returns(_ownerPerson);

//            var friendCommandMock = _kernel.GetMock<IFriendCommand>();


//            var friendQueryMock = _kernel.GetMock<IFriendQuery>();
//            var existingFriend = new Friend() { FriendGuid = friendPerson.PersonGuid, OwnerGuid = _ownerPerson.PersonGuid, IsDeleted = true }; // *** TRUE ***
//            friendQueryMock.Setup(mock => mock.GetFriend(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(existingFriend);

//            var friendRepositoryLogicMock = _kernel.GetMock<IFriendRepositoryLogic>();

//            var transactionQueryMock = _kernel.GetMock<ITransactionQuery>();
//            transactionQueryMock.Setup(mock => mock.IsPersonUsedInAnyPosts(It.IsAny<Guid>())).Returns(false);

//            var deleteOfflineFriendLogicMock = _kernel.GetMock<IDeleteFriendCommand>();
//            deleteOfflineFriendLogicMock.Setup(mock => mock.UnDeleteOnlineFriend(friendPerson.PersonGuid, _ownerPerson.PersonGuid));


//            var friendRepository = _kernel.Get<IFriendRepository>();
//            const bool isInUse = true; // meaning the the friend is in use on the App
//            friendRepository.DeleteOrUndeleteOnlineFriend(username, personGuid, displayname, isInUse, isFriendDeleted);

//            personCommandMock.VerifyAll();
//            personQueryMock.VerifyAll();
//            friendCommandMock.VerifyAll();
//            credentialQueryMock.VerifyAll();
//            friendQueryMock.VerifyAll();
//            friendRepositoryLogicMock.VerifyAll();
//            deleteOfflineFriendLogicMock.VerifyAll();

//            personQueryMock.Verify(a => a.GetPerson(It.IsAny<Guid>()), Times.Never());
//            personCommandMock.Verify(a => a.SavePerson(It.IsAny<Person>()), Times.Never());
//            friendRepositoryLogicMock.Verify(a => a.MapToFriend(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>()), Times.Never);
//            friendCommandMock.Verify(a => a.SaveFriend(It.IsAny<Friend>()), Times.Never);
//            deleteOfflineFriendLogicMock.Verify(a => a.DeleteOfflineFriend(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
//            transactionQueryMock.Verify(a => a.IsPersonUsedInAnyPosts(It.IsAny<Guid>()), Times.Never);
//        }


//    }
//}

