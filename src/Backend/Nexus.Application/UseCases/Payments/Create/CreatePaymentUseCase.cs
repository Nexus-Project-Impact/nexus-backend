using AutoMapper;
using Nexus.Communication.Responses;
using Nexus.Communication.Requests;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories.Payments;
using Nexus.Domain.Repositories;

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
        
        public async Task<ResponsePayment> Execute(RequestPayment request)
        {
            var payment = _mapper.Map<Payment>(request);

            await _repository.AddAsync(payment);
            await _unitOfWork.Commit();

            return new ResponsePayment
            {
                Id = payment.Id,
                AmountPaid = payment.AmountPaid,
                Status = payment.Status,
                PaymentDate = payment.Date
            };
        }
    }
}