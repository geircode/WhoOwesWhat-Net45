using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhoOwesWhat.Domain.DTO
{
    public class UserCredential
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }

        public Person Person { get; set; }
    }


    public class AddUserCredentialModel
    {
        public Guid PersonGuid { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }
}
