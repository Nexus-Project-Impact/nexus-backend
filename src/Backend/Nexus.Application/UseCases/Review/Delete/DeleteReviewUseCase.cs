using Nexus.Domain.Entities;
using Nexus.Domain.Repositories;
using Nexus.Domain.Repositories.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Application.UseCases.Review.Delete
{
    public class DeleteReviewUseCase : IDeleteReviewUseCase
    {
        private readonly IReviewRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteReviewUseCase(IReviewRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> ExecuteDelete(int id)
        {
            var review = await _repository.GetByIdAsync(id);
            if (review == null)
            {
                return false;
            }

            await _repository.DeleteAsync(id);

            await _unitOfWork.Commit();

            return true;
        }
    }
}
