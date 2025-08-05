using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Domain.DTOs
{
    public class PaymentDto
    {
        public int ReservationId { get; set; }
        public double AmountPaid { get; set; }
        public string Receipt { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public string PaymentMethod { get; set; } = string.Empty;

        public string Status { get; set; }
    }
}
