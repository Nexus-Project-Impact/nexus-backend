using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Domain.Entities
{
    public class Travelers
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? RG { get; set; }
        public int ReservationId { get; set; }
        public Reservation Reservation { get; set; }
    }
}
