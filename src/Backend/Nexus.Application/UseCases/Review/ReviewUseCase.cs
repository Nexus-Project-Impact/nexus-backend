using AutoMapper;
using Nexus.Communication.Responses;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Application.UseCases.Review
{
    public class ReviewUseCase : IReviewUseCase
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public ReviewUseCase(IReviewRepository reviewRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ResponseReviewJson>> Execute()
        {

            var reviews = await _reviewRepository.GetAllAsync();

            var reviewsJson = _mapper.Map<IEnumerable<ResponseReviewJson>>(reviews);

            return reviewsJson;

        }
    }
}
