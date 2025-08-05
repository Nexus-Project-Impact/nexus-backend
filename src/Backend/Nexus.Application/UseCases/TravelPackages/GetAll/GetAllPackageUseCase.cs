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

namespace Nexus.Application.UseCases.Packages.GetAll
{
    public class GetAllPackageUseCase : IGetAllPackageUseCase
    {
        private readonly IPackageRepository<TravelPackage, int> _repository;
        private readonly IMapper _mapper;


        public GetAllPackageUseCase(IPackageRepository<TravelPackage, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ResponsePackage>> ExecuteGetAll()
        {
            var packages = await _repository.GetAllAsync();
            var packagesJson = _mapper.Map<IEnumerable<ResponsePackage>>(packages);
            return packagesJson;
        }
    }
}
