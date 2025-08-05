using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Domain.Entities
{
    public class Payment
        {
        public int Id { get; set; }

        public int ReservationId { get; set; }

        public Reservation? Reservation { get; set; } // Corrigido para PascalCase

        public double AmountPaid { get; set; }

        public string? PaymentMethod { get; set; }

        public DateTime Date { get; set; }

        public string? Status { get; set; }

        public string? Receipt { get; set; }

    }
}
