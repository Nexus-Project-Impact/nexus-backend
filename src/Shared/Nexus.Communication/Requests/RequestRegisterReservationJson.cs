using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Communication.Requests
{
    public class RequestRegisterReservationJson
    {
        public string? UserId { get; set; }
        public int TravelPackageId { get; set; }
        public ICollection<RequestTravelers> Traveler { get; set; } = new List<RequestTravelers>();

    }
}
