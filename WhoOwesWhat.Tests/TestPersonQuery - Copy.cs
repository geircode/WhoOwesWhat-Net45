using System;
using System.Collections.Generic;
using WhoOwesWhat.Domain.DTO;
using WhoOwesWhat.Service.DTO;

namespace WhoOwesWhat.Tests
{

    public interface ITestUserCredentialQuery
    {
    }

    public class TestUserCredentialQuery : ITestUserCredentialQuery
    {

        public static UserCredential GeirUserCredential = new UserCredential
        {
            Username = "smurf@smurfmail.com",
            Email = "smurf@smurfmail.com",
            PasswordHash = "0C6AD70BEB3A7E76C3FC7ADAB7C46ACC",  // Good
            Person = TestPersonQuery.GeirPersonDomain
        };
    }
}
