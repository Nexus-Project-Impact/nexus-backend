using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Domain.Entities
{
    public class Review
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; }
        public User User { get; set; }
        public string PackageId { get; set; }
        public int Rating { get; set; } 
        public string? Comment { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 

        
    }
}
