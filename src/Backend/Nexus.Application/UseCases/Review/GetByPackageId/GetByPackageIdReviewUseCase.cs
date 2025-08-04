using AutoMapper;
using Nexus.Communication.Responses;
using Nexus.Domain.Repositories.Review;

namespace Nexus.Application.UseCases.Review.GetByPackageId
{
    public class GetByPackageIdReviewUseCase : IGetByPackageIdReviewUseCase
    {
        private readonly IReviewRepository _repository;
        private readonly IMapper _mapper;

        public GetByPackageIdReviewUseCase(IReviewRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ResponseReviewJson>> ExecuteGetByPackageId(int packageId)
        {
            var reviews = await _repository.GetByPackageIdAsync(packageId);
            var reviewsJson = _mapper.Map<IEnumerable<ResponseReviewJson>>(reviews);
            return reviewsJson;
        }
    }
}