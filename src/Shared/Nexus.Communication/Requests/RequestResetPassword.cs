using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Communication.Requests
{
    public class RequestResetPassword
    {
        public string? email { get; set; }

        public string? code { get; set; }

        public string? newPassword { get; set; }

    }
}
