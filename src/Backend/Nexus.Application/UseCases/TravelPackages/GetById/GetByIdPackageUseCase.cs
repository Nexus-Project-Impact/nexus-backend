using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Nexus.Communication.Responses;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories;
using Nexus.Domain.Repositories.Packages;

namespace Nexus.Application.UseCases.Packages.GetById
{
    public class GetByIdPackageUseCase : IGetByIdPackageUseCase
    {
        private readonly IPackageRepository<TravelPackage, int> _repository;
        private readonly IMapper _mapper;

        public GetByIdPackageUseCase(IPackageRepository<TravelPackage, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task <ResponsePackage?> ExecuteGetById(int id)
        {
            var packages = await _repository.GetByIdAsync(id);
            var packagesJson = _mapper.Map<ResponsePackage>(packages);
            return packagesJson;
        }

    }
}
