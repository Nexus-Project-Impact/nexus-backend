using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Nexus.Communication.Requests;
using Nexus.Communication.Responses;
using Nexus.Domain.Repositories;

namespace Nexus.Application.UseCases.TravelPackages.Update
{
    public class UpdatePackageUseCase : IUpdatePackageUseCase
    {
        private readonly IRepository<TravelPackageEntity, int> _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePackageUseCase(IRepository<TravelPackageEntity, int> repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseRegisteredPackageJson?> ExecuteUpdate(int id, RequestUpdatePackageJson register)
        {
            var package = await _repository.GetByIdAsync(id);

            if (package == null)
            {
                return null;
            }

            _mapper.Map(register, package);

            await _repository.UpdateAsync(package);

            var packagesJson = _mapper.Map<ResponseRegisteredPackageJson>(package);

            await _unitOfWork.Commit();

            return packagesJson;
        }
    }
}
