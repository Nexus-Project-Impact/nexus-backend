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

        public async Task<ResponseRegisteredReviewJson> Execute(RequestRegisterReviewJson request)
        {
            var review = _mapper.Map<Domain.Entities.Review>(request);
            await _repository.AddAsync(review);
            await _unitOfWork.Commit();

            return new ResponseRegisteredReviewJson
            {
                Id = review.Id,
                PackageId = review.PackageId,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt,
                Mensage = "Avaliação registrada com sucesso!"
            };

        }
    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RequestRegisterReviewJson, Domain.Entities.Review>();
        }
    }
}
