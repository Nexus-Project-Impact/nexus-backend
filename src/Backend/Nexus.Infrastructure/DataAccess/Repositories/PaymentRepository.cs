using Microsoft.EntityFrameworkCore;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories;
using Nexus.Domain.Repositories.Payments;

namespace Nexus.Infrastructure.DataAccess.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly NexusDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentRepository(NexusDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
        }

        public async Task DeleteAsync(int id)
        {
            await _context.Payments
                .Where(p => p.Id == id).ExecuteDeleteAsync();
        }

        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
           return await _context.Payments.ToListAsync();
        }

        public async Task<Payment> GetByIdAsync(int id)
        {
            return await _context.Payments.FindAsync(id) 
                   ?? throw new KeyNotFoundException($"Pagamento com id{id} não identificado");
        }
    }
}
