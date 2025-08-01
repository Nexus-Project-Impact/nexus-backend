using Nexus.Communication.Requests;
using Nexus.Communication.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Application.Services.Payments
{
    public interface IPaymentService
    {
        public Task<ResponseBoleto> PayWithBoleto(string userId, RequestPayment request);
    }
}
