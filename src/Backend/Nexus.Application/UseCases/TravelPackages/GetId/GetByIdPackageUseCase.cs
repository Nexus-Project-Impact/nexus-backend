using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Nexus.Communication.Responses;
using Nexus.Domain.Repositories;

namespace Nexus.Application.UseCases.TravelPackages.GetId
{
    public class GetByIdPackageUseCase : IGetByIdPackageUseCase
    {
        private readonly IRepository<TravelPackageEntity, int> _repository;
        private readonly IMapper _mapper;

        public GetByIdPackageUseCase(IRepository<TravelPackageEntity, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ResponsePackageJson?> ExecuteGetById(int id)
        {
            var packages = await _repository.GetByIdAsync(id);

            var packagesJson = _mapper.Map<ResponsePackageJson>(packages);

            return packagesJson;    
        }
    }
}
