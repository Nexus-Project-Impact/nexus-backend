using AutoMapper;
using Nexus.Communication.Responses;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories.Payments;
using Nexus.Domain.Repositories;
using Nexus.Domain.DTOs;

namespace Nexus.Application.UseCases.Payments.Create    
{
    public class CreatePaymentUseCase : ICreatePaymentUseCase
    {
        private readonly IPaymentRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreatePaymentUseCase(IPaymentRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        
        public async Task<PaymentDto> Execute(PaymentDto request)
        {
            var payment = _mapper.Map<Payment>(request);

            await _repository.AddAsync(payment);
            await _unitOfWork.Commit();

            return new PaymentDto
            {
                AmountPaid = payment.AmountPaid,
                Status = payment.Status,  
                Receipt = payment.Receipt,
                Date = payment.Date,
            };
        }
    }
}