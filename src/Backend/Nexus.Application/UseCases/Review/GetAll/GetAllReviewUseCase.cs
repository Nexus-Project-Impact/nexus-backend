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

namespace Nexus.Application.UseCases.Review.GetAll
{
    public class GetAllReviewUseCase : IGetAllReviewUseCase
    {
        private readonly IReviewRepository _repository;
        private readonly IMapper _mapper;

        public GetAllReviewUseCase(IReviewRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task<IEnumerable<ResponseReviewJson>> ExecuteGetAll()
        {
            var reviews = await _repository.GetAllAsync();

            var reviewsJson = _mapper.Map<IEnumerable<ResponseReviewJson>>(reviews);

            return reviewsJson;
        }
    }
}
