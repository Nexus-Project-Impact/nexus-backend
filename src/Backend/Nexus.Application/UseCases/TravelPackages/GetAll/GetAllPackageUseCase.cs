using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Nexus.Communication.Responses;
using Nexus.Domain.Repositories;

namespace Nexus.Application.UseCases.TravelPackages.GetAll
{
    public class GetAllPackageUseCase : IGetAllPackageUseCase
    {
        private readonly IRepository<TravelPackageEntity, int> _repository;
        private readonly IMapper _mapper;
        

        public GetAllPackageUseCase(IRepository<TravelPackageEntity, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ResponsePackageJson>> ExecuteGetAll()
        {
            var packages = await _repository.GetAllAsync();

            var packagesJson = _mapper.Map<IEnumerable<ResponsePackageJson>>(packages);

            return packagesJson;
        }
    }
}
