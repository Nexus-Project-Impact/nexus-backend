using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Communication.Requests
{
    public class RequestModerateReviewJson
    {
        public int ReviewId { get; set; }
        public string? Reason { get; set; }
        public string NewComment { get; set; } // Required field for the new moderated comment
    }
}
