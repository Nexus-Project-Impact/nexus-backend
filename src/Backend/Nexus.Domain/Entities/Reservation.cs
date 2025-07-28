using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Domain.Entities
{
    public class Reservation
    {
        public int Id { get; set; }
        public DateTime ReservationDate { get; set; } = DateTime.UtcNow;
        public string? Status { get; set; }
        public int ReservationNumber { get; set; }
        public string? UserId { get; set; }
        public int TravelPackageId { get; set; }
        public User? User { get; set; }
        public TravelPackage? TravelPackageEntity { get; set; }
        public ICollection<Travelers> Traveler { get; set; } = new List<Travelers>();
    }
}
