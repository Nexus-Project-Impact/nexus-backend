using Nexus.Communication.Requests;
using Nexus.Communication.Responses;
using Nexus.Domain.Repositories;
using Nexus.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Nexus.Application.UseCases.Review.Register
{
    public class RegisterReviewUseCase : IRegisterReviewUseCase
    {
        private readonly IRepository<Domain.Entities.Review, int> _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterReviewUseCase(
            IRepository<Domain.Entities.Review, int> repository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseRegisteredReviewJson> Execute(RequestRegisterReviewJson request, string userId)
        {
            var review = _mapper.Map<Domain.Entities.Review>(request);
            review.UserId = userId; // Definir o UserId a partir do token
            
            await _repository.AddAsync(review);
            await _unitOfWork.Commit();

            var response = _mapper.Map<ResponseRegisteredReviewJson>(review);
            response.Mensage = "Review registered successfully!";
            
            return response;
        }
    }
}
