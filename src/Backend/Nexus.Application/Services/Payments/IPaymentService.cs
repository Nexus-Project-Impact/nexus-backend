using Nexus.Communication.Requests;
using Nexus.Communication.Responses;
using Nexus.Domain.DTOs;
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

        public Task<ResponsePix> PayWithPix(string userId, RequestPayment request);

        public Task<ResponsePayment> PayWithCard(string userId, RequestPayment request);


        public Task<IEnumerable<PaymentDto>> GetPayments(string userId);
    }
}
    