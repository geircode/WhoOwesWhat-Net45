using System;
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
    public class DeleteOfflineFriendLogicTests
    {
        private static MoqMockingKernel _kernel;
        Person _ownerPerson;

        public DeleteOfflineFriendLogicTests()
        {
            _kernel = new MoqMockingKernel();
            _kernel.Bind<IDeleteFriendCommand>().To<DeleteFriendCommand>();
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

        //[Test]
        //public void When_sync_after_deleting_OfflineFriend_on_App_1()
        //{
        //    /*
        //     Action: Delete Friend Marianne

        //     Before:
        //        Geir legger til Marianne som OfflineFriend på App
        //        Geir synkroniserer  
        //        Geir sletter Marianne på App (Marianne er ikke i bruk i App)
        //        Geir synkroniserer
             
        //     After:
        //     Friend(Marianne) and Person(Marianne) is marked IsDeleted = True on the server

        //     */

        //    Guid personGuid = new Guid("14DD5F5A-4747-4747-4747-64A821B1EAA0");
        //    const string displayname = "Marianne";
        //    const bool isFriendDeleted = true; // *** TRUE ***

        //    var friendPerson = new Person()
        //    {
        //        PersonGuid = personGuid,
        //        Displayname = displayname,
        //        IsDeleted = false
        //    };

        //    const string username = "smurf@smurf.com";

        //    var credentialQueryMock = _kernel.GetMock<IUserCredentialQuery>();
        //    credentialQueryMock.Setup(mock => mock.IsNotOnlinePerson(It.IsAny<Guid>())).Returns(true);

        //    var personCommandMock = _kernel.GetMock<IPersonCommand>();
        //    personCommandMock.Setup(mock => mock.DeletePerson(It.IsAny<Guid>()));

        //    var friendCommandMock = _kernel.GetMock<IFriendCommand>();
        //    friendCommandMock.Setup(mock => mock.DeleteFriend(It.IsAny<Guid>(), It.IsAny<Guid>()));


        //    var transactionQueryMock = _kernel.GetMock<ITransactionQuery>();
        //    transactionQueryMock.Setup(mock => mock.IsPersonUsedInAnyPosts(It.IsAny<Guid>())).Returns(false);

        //    var logic = _kernel.Get<IDeleteFriendCommand>();
        //    logic.DeleteOfflineFriend(personGuid, _ownerPerson.PersonGuid);

        //    transactionQueryMock.VerifyAll();
        //    personCommandMock.VerifyAll();
        //    friendCommandMock.VerifyAll();
        //}

        //[Test]
        //public void When_sync_after_deleting_OfflineFriend_on_App_3()
        //{
        //    /*
        //     Action: Delete Friend Marianne

        //     Before:
        //        Geir legger til Marianne som OfflineFriend på App
        //        Geir synkroniserer  
        //        Geir sletter Marianne på App (Marianne er ikke i bruk i App)
        //        Geir synkroniserer
             
        //     After:
        //     Friend(Marianne) and Person(Marianne) is marked IsDeleted = True on the server

        //     */

        //    Guid personGuid = new Guid("14DD5F5A-4747-4747-4747-64A821B1EAA0");
        //    const string displayname = "Marianne";
        //    const bool isFriendDeleted = true; // *** TRUE ***

        //    var friendPerson = new Person()
        //    {
        //        PersonGuid = personGuid,
        //        Displayname = displayname,
        //        IsDeleted = false
        //    };

        //    const string username = "smurf@smurf.com";

        //    var credentialQueryMock = _kernel.GetMock<IUserCredentialQuery>();
        //    credentialQueryMock.Setup(mock => mock.IsNotOnlinePerson(It.IsAny<Guid>())).Returns(true);

        //    var personCommandMock = _kernel.GetMock<IPersonCommand>();
        //    personCommandMock.Setup(mock => mock.UnDeletePerson(friendPerson.PersonGuid));

        //    var friendCommandMock = _kernel.GetMock<IFriendCommand>();
        //    friendCommandMock.Setup(mock => mock.UnDeleteFriend(friendPerson.PersonGuid, _ownerPerson.PersonGuid));

        //    var transactionQueryMock = _kernel.GetMock<ITransactionQuery>();

        //    var logic = _kernel.Get<IDeleteFriendCommand>();
        //    logic.UndeleteOfflineFriend(personGuid, _ownerPerson.PersonGuid);

        //    personCommandMock.VerifyAll();
        //    friendCommandMock.VerifyAll();
        //}



        [Test]
        public void When_sync_after_deleting_OfflineFriend_on_App_2()
        {
            /*
             Action: Delete Friend Marianne

             Before:
                Geir legger til Marianne som OfflineFriend på Mobil(id=*1*)
                Geir synkroniserer Mobil(id=*1*)
                Geir logger inn på Mobil(id=*2*) og synkroniserer
                Geir legger til Marianne som Payer på en POST på Mobil(id=2)
                Geir synkroniserer Mobil(id=*2*)
                Geir sletter Marianne på Mobil(id=*1*)
                Geir synkroniserer
             
             After:
             Siden Marianne er i bruk på server så har server presedens over mobil(id=1) og legger Marianne inn som Friend igjen.

             */

        }


    }
}

