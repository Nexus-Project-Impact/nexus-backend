using Nexus.Communication.Requests;
using Nexus.Communication.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Application.UseCases.Payments.Create
{
    public interface ICreatePaymentUseCase
    {
        public Task<ResponsePayment> Execute(RequestPayment request);
    }
}
