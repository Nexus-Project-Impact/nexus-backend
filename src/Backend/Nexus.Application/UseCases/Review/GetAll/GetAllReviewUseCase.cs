using AutoMapper;
using Nexus.Communication.Responses;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Application.UseCases.Review.GetAll
{
    public class GetAllReviewUseCase : IGetAllReviewUseCase
    {
        private readonly IRepository<Domain.Entities.Review, int> _repository;
        private readonly IMapper _mapper;

        public GetAllReviewUseCase(IRepository<Domain.Entities.Review, int> repository, IMapper mapper)
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
