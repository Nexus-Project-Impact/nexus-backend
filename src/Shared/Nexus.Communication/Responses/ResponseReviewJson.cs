using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Communication.Responses
{
    public class ResponseReviewJson
    {
        public string Id { get; set; } 
        public int Rating { get; set; } 
        public string? Comment { get; set; } 
        public DateTime CreatedAt { get; set; } 
    }
}
