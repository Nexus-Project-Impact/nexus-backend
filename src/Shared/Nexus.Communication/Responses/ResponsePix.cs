using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Communication.Responses
{
    public class ResponsePix
    {
        public string TransactionId { get; set; }
        public double Amount { get; set; }
        public string PayerName { get; set; }
        public string QrCode { get; set; }
        public string QrCodeImageUrl { get; set; }
    }
}
