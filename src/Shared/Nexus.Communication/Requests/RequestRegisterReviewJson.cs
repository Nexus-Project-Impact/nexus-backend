using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Communication.Requests
{
    public class RequestRegisterReviewJson
    {
        public string UserId { get; set; }
        public string PackageId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
