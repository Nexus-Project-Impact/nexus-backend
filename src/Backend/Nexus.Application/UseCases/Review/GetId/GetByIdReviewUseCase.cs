using AutoMapper;
using Nexus.Communication.Responses;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories;
using Nexus.Domain.Repositories.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Application.UseCases.Review.GetId
{
    public class GetByIdReviewUseCase : IGetByIdReviewUseCase
    {
        private readonly IReviewRepository _repository;
        private readonly IMapper _mapper;

        public GetByIdReviewUseCase(IReviewRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ResponseReviewJson?> ExecuteGetById(int id)
        {
            var review = await _repository.GetByIdAsync(id);

            var reviewJson = _mapper.Map<ResponseReviewJson>(review);

            return reviewJson;
        }
    }
}
