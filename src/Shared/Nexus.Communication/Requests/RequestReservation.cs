using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nexus.Domain.DTOs;

namespace Nexus.Communication.Requests
{
    public class RequestReservation
    {
        public int Id { get; set; }
        public DateTime ReservationDate { get; set; } = DateTime.UtcNow;
        public string? Status { get; set; }
        public int ReservationNumber { get; set; }
        public string? UserId { get; set; }
        public int TravelPackageId { get; set; }
        public ICollection<RequestTravelers> Traveler { get; set; } = new List<RequestTravelers>();

    }
}
