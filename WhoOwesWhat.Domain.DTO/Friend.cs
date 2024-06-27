using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhoOwesWhat.Domain.DTO
{
    public class Friend
    {
        public bool IsDeleted { get; set; }
        public Guid OwnerGuid { get; set; }
        public Guid FriendGuid { get; set; }
    }

    public class GetFriendsToAppModel
    {
        public Guid OwnerGuid { get; set; }
        public Guid FriendGuid { get; set; }
        public string Displayname { get; set; }
        public bool IsFriendOnlinePerson { get; set; }
        public bool IsFriendDeleted { get; set; }
        public bool IsPersonDeleted { get; set; }
    }
}
