using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Nexus.Communication.Responses;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories.Packages;

namespace Nexus.Application.UseCases.Packages.GetByDepartureDate
{
    public class GetByDepartureDatePackageUseCase : IGetByDepartureDatePackageUseCase
    {
        private readonly IPackageRepository<TravelPackage, int> _repository;
        private readonly IMapper _mapper;

        public GetByDepartureDatePackageUseCase(IPackageRepository<TravelPackage, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ResponsePackage?>> ExecuteGetByDepartureDate(DateTime initialDate, DateTime finalDate)
        {
            var packages =  await _repository.GetByDepartureDateAsync(initialDate, finalDate);
            var packagesJson = _mapper.Map<IEnumerable<ResponsePackage?>>(packages);
            return packagesJson;
        }

    }
}
