using Nexus.Communication.Requests;
using Nexus.Communication.Responses;
using Nexus.Domain.Repositories;
using Nexus.Domain.Repositories.Review;
using System.Threading.Tasks;
using AutoMapper;

namespace Nexus.Application.UseCases.Review.Moderate
{
    public class ModerateReviewUseCase : IModerateReviewUseCase
    {
        private readonly IReviewRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ModerateReviewUseCase(
            IReviewRepository repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseModeratedReviewJson> Execute(RequestModerateReviewJson request)
        {
            var review = await _repository.GetByIdAsync(request.ReviewId);

            if (review == null)
            {
                return null;
            }

            await _repository.UpdateAsync(review);
            await _unitOfWork.Commit();

            return new ResponseModeratedReviewJson
            {
                ReviewId = review.Id,
                ActionTaken = request.Action.ToLower(),
                Mensagem = "Avaliação moderada com sucesso."
            };
        }
    }
}
