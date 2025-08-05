using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories;
using Nexus.Domain.Repositories.Packages;

namespace Nexus.Application.UseCases.Packages.Delete
{
    public class DeletePackageUseCase : IDeletePackageUseCase
    {
        private readonly IPackageRepository<TravelPackage, int> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeletePackageUseCase(IPackageRepository<TravelPackage, int> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> ExecuteDelete(int id)
        {
            var package = await _repository.GetByIdAsync(id);

            if (package == null)
                return false;

            if (package.IsActive)
            {
                package.IsActive = false;
                await _repository.UpdateAsync(package);
                await _unitOfWork.Commit();
                return true;
            }

            return false;
        }
    }
}
