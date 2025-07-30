using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Nexus.Communication.Responses;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories.Packages;

namespace Nexus.Application.UseCases.Packages.GetByDestination
{
    public class GetByDestinationPackageUseCase :IGetByDestinationPackageUseCase
    {
        private readonly IPackageRepository<TravelPackage, int> _repository;
        private readonly IMapper _mapper;

        public GetByDestinationPackageUseCase(IPackageRepository<TravelPackage, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ResponsePackage?>>ExecuteGetByDestination(string destination)
        {
            if (string.IsNullOrWhiteSpace(destination)) 
                return new List<ResponsePackage?>();

            var packages = await _repository.GetByDestinationAsync(destination);

            var packagesJson = _mapper.Map<IEnumerable<ResponsePackage?>>(packages);

            return packagesJson;
        }
    }
}
