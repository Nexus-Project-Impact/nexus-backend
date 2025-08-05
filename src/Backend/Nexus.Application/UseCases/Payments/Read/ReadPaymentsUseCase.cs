using AutoMapper;
using Nexus.Domain.DTOs;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories.Payments;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexus.Application.UseCases.Payments.Read
{
    public class ReadPaymentsUseCase : IReadPaymentUseCase
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;

        public ReadPaymentsUseCase(IPaymentRepository paymentRepository, IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
        }

        public async Task<PaymentDto> GetById(int id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            return _mapper.Map<PaymentDto>(payment);
        }

        public async Task<IEnumerable<PaymentDto>> GetAllByUserId(string userId)
        {
            var allPayments = await _paymentRepository.GetAllAsync();

            var filtered = allPayments
                .Where(p => p.Reservation != null && p.Reservation.UserId != null && p.Reservation.UserId == userId)
                .ToList();
            return _mapper.Map<IEnumerable<PaymentDto>>(filtered);
        }
    }
}
