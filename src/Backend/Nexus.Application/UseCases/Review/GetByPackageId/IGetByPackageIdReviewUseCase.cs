using Nexus.Communication.Responses;

namespace Nexus.Application.UseCases.Review.GetByPackageId
{
    public interface IGetByPackageIdReviewUseCase
    {
        Task<IEnumerable<ResponseReviewJson>> ExecuteGetByPackageId(int packageId);
    }
}