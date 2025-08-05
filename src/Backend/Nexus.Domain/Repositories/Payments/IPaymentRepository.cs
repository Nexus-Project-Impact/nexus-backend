using Nexus.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Domain.Repositories.Payments
{
    public interface IPaymentRepository
    {
        Task AddAsync(Payment payment);

        Task DeleteAsync(int id);

        Task<IEnumerable<Payment>> GetAllAsync();

        Task<Payment> GetByIdAsync(int id); 

    }
}
