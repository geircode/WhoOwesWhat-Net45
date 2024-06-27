using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhoOwesWhat.Domain.DTO
{
    public class Friendrequest
    {
        public Guid PersonRequestedGuid { get; set; }
        public Guid RequesterPersonGuid { get; set; }
    }

    public class SaveFriendrequestModel
    {
        public Guid PersonRequestedGuid { get; set; }
        public Guid RequesterPersonGuid { get; set; }
    }

}
