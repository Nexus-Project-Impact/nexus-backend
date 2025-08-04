using Nexus.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Application.UseCases.Payments.Read
{
    public interface IReadPaymentUseCase
    {

       public Task<PaymentDto> GetById(int id);

       public Task<IEnumerable<PaymentDto>> GetAllByUserId(string userId);
    }
}
