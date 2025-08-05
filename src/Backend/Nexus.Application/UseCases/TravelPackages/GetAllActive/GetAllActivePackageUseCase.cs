using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Nexus.Communication.Responses;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories.Packages;

namespace Nexus.Application.UseCases.TravelPackages.GetAllActive
{
    public class GetAllActivePackageUseCase : IGetAllActivePackageUseCase
    {
        private readonly IPackageRepository<TravelPackage, int> _repository;
        private readonly IMapper _mapper;


        public GetAllActivePackageUseCase(IPackageRepository<TravelPackage, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ResponsePackage>> ExecuteGetAllActive()
        {
            var packages = await _repository.GetAllActiveAsync();
            var packagesJson = _mapper.Map<IEnumerable<ResponsePackage>>(packages);
            return packagesJson;
        }
    }
}
