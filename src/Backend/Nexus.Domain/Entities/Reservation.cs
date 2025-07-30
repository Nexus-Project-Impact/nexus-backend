using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Domain.Entities
{
    public class Reservation
    {
        public int Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime ReservationDate { get; set; } = DateTime.UtcNow.Date;
        public int ReservationNumber { get; set; }
        public string? UserId { get; set; }
        public int TravelPackageId { get; set; }
        public User? User { get; set; }
        public TravelPackage? TravelPackage { get; set; }
        public ICollection<Travelers> Traveler { get; set; } = new List<Travelers>();
    }
}
