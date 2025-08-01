using Nexus.Communication.Requests;
using Nexus.Communication.Responses;
using Nexus.Domain.Repositories;
using System.Threading.Tasks;
using AutoMapper;

namespace Nexus.Application.UseCases.Review.Moderate
{
    public class ModerateReviewUseCase : IModerateReviewUseCase
    {
        private readonly IRepository<Domain.Entities.Review, int> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ModerateReviewUseCase(
            IRepository<Domain.Entities.Review, int> repository,
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
