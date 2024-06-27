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
    public class GetFriendrequestsTests
    {
        private static MoqMockingKernel _kernel;
        private Person _ownerPerson;

        public GetFriendrequestsTests()
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
        public void When_getting_friendrequests_to_a_person__it_should_succeed()
        {
            /*
             
                Action: Get friendrequests to a Person

                Before:
                Geir legger til Marianne og Beate som Online friends
                
                After:
                Get a list of friends as UserCredentials

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

            const string username = "smurf@smurf.com";

            var personQueryMock = _kernel.GetMock<IPersonQuery>();
            personQueryMock.Setup(mock => mock.GetPersonByUsername(username)).Returns(_ownerPerson);

            var friendrequestQueryMock = _kernel.GetMock<IFriendrequestQuery>();
            var friendrequests = new List<Friendrequest>();
            var marianneRequest = new Friendrequest()
            {
                PersonRequestedGuid = _ownerPerson.PersonGuid,
                RequesterPersonGuid = friendBeate.PersonGuid
            };
            var beateRequest = new Friendrequest()
            {
                PersonRequestedGuid = _ownerPerson.PersonGuid,
                RequesterPersonGuid = friendMarianne.PersonGuid
            };

            friendrequests.Add(marianneRequest);
            friendrequests.Add(beateRequest);

            friendrequestQueryMock.Setup(mock => mock.GetFriendrequestsByPersonRequested(_ownerPerson.PersonGuid)).Returns(friendrequests);

            var userCredentialQueryMock = _kernel.GetMock<IUserCredentialQuery>();
            userCredentialQueryMock.Setup(mock => mock.GetUserCredentialByPersonGuid(marianneRequest.RequesterPersonGuid)).Returns(marianneCredential);
            userCredentialQueryMock.Setup(mock => mock.GetUserCredentialByPersonGuid(beateRequest.RequesterPersonGuid)).Returns(beateCredential);

            var friendrequestRepository = _kernel.Get<IFriendrequestRepository>();
            var friendrequestsToPerson = friendrequestRepository.GetFriendsrequestsToPerson(username);

            List<UserCredential> friendrequestsToPersonExpected = new List<UserCredential>();
            friendrequestsToPersonExpected.Add(marianneCredential);
            friendrequestsToPersonExpected.Add(beateCredential);

            friendrequestsToPerson.ShouldAllBeEquivalentTo(friendrequestsToPersonExpected);

            personQueryMock.VerifyAll();
            friendrequestQueryMock.VerifyAll();
        }  
        
        
    }
}

