using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Nexus.Communication.Responses;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories.Packages;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Nexus.Application.UseCases.Packages.GetByValue
{
    public class GetByValuePackageUseCase : IGetByValuePackageUseCase
    {
        private readonly IPackageRepository<TravelPackage, int> _repository;
        private readonly IMapper _mapper;

        public GetByValuePackageUseCase(IPackageRepository<TravelPackage, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ResponsePackage?>> ExecuteGetByValue(double minValue, double maxValue)
        {
            var packages = await _repository.GetByValueAsync(minValue, maxValue);

            var packagesJson = _mapper.Map<IEnumerable<ResponsePackage?>>(packages);

            return packagesJson;
        }
    }
}
