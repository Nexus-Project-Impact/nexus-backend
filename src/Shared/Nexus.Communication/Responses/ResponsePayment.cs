using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Communication.Responses
{
    public class ResponsePayment
    {

        public int Id { get; set; }
        public double AmountPaid { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? Status { get; set; }

    }
}
