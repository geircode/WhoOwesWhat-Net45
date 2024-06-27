using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.Domain.DTO;
using WhoOwesWhat.Domain.Interfaces;
using WhoOwesWhat.Service.DTO;

namespace WhoOwesWhat.Service.Controller
{
    public interface IUserController
    {
        AuthenticateUserResponse AuthenticateUser(AuthenticateUserRequest request);
        BasicResponse CreateUser(CreateUserRequest createUserRequest);
        GetPersonByEmailResponse GetPersonByEmail(GetPersonByEmailRequest request);
    }

    public class UserController : IUserController
    {
        private readonly IUserRepository _userRepository;
        private readonly IPersonQuery _personQuery;
        private readonly IUserCredentialQuery _userCredentialQuery;

        public UserController(IUserRepository userRepository, IPersonQuery personQuery, IUserCredentialQuery userCredentialQuery)
        {
            _userRepository = userRepository;
            _personQuery = personQuery;
            _userCredentialQuery = userCredentialQuery;
        }

        public AuthenticateUserResponse AuthenticateUser(AuthenticateUserRequest request)
        {
            var response = new AuthenticateUserResponse();

            var isAuthenticated = _userRepository.AuthenticateUser(request.username, request.password);
            if (isAuthenticated)
            {
                var person = _personQuery.GetPersonByUsername(request.username);
                response.isSuccess = true;
                response.personGuid = person.PersonGuid;
                response.displayname = person.Displayname;
                response.mobil = person.Mobile;
            }

            return response;
        }

        public BasicResponse CreateUser(CreateUserRequest request)
        {
            _userRepository.CreateUser(request.personGuid, request.displayname, request.username, request.email, request.mobil, request.password);
            return new BasicResponse() { isSuccess = true };
        }

        public GetPersonByEmailResponse GetPersonByEmail(GetPersonByEmailRequest request)
        {
            var response = new GetPersonByEmailResponse();

            var isAuthenticated = _userRepository.AuthenticateUser(request.username, request.password);
            if (isAuthenticated)
            {
                // Email == Username
                var credential = _userCredentialQuery.GetUserCredential(request.emailCriteria);
                if (credential != null)
                {
                    response.username = credential.Username;
                    response.displayname = credential.Person.Displayname;
                    response.isSuccess = true;
                }
            }

            return response;
        }

    }
    
}