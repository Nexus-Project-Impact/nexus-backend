using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Communication.Responses
{
    public class ResponseRegisteredReviewJson
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int PackageId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Mensage { get; set; } = "Review registered successfully!";
    }
}
