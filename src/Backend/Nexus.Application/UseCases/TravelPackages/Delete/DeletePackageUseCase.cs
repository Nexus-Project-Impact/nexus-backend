using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Nexus.Domain.Repositories;

namespace Nexus.Application.UseCases.TravelPackages.Delete
{
    public class DeletePackageUseCase : IDeletePackageUseCase
    {
        private readonly IRepository<TravelPackageEntity, int> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeletePackageUseCase(IRepository<TravelPackageEntity, int> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> ExecuteDelete(int id)
        {
            var package = await _repository.GetByIdAsync(id);
            if (package == null)
            {
                return false;
            }

            await _repository.DeleteAsync(id);

            await _unitOfWork.Commit();

            return true;
        }
    }
}
