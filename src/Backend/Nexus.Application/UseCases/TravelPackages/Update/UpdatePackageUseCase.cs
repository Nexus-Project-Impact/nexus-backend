using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Nexus.Communication.Requests;
using Nexus.Communication.Responses;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories;
using Nexus.Domain.Repositories.Packages;

namespace Nexus.Application.UseCases.Packages.Update
{
    public class UpdatePackageUseCase : IUpdatePackageUseCase
    {
        private readonly IPackageRepository<TravelPackage, int> _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePackageUseCase(IPackageRepository<TravelPackage, int> repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponsePackage?> ExecuteUpdate(int id, RequestUpdatePackage register)
        {
            var package = await _repository.GetByIdAsync(id);

            if (package == null)
            {
                return null;
            }

            _mapper.Map(register, package);

            await _repository.UpdateAsync(package);

            var packagesJson = _mapper.Map<ResponsePackage>(package);

            await _unitOfWork.Commit();

            return packagesJson;
        }

    }
}
