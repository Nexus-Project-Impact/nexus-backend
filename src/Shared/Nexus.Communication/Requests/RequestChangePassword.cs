using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Communication.Requests
{
    public class RequestChangePassword
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
