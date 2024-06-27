using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhoOwesWhat.Domain.DTO
{
    public class Person
    {
        public Guid PersonGuid { get; set; }
        public string Displayname { get; set; }
        public string Mobile { get; set; }
        public bool IsDeleted { get; set; }
    }
}
