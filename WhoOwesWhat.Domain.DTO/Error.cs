using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhoOwesWhat.Domain.DTO
{
    public class Error
    {
        public DateTime Created { get; set; }
        public string Message { get; set; }
        public string ErrorJson { get; set; }
    }
}
