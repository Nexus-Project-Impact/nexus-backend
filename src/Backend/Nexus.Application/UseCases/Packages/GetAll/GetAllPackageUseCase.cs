using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Nexus.Communication.Responses;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories;

namespace Nexus.Application.UseCases.Packages.GetAll
{
    public class GetAllPackageUseCase : IGetAllPackageUseCase
    {
        private readonly IRepository<TravelPackage, int> _repository;
        private readonly IMapper _mapper;


        public GetAllPackageUseCase(IRepository<TravelPackage, int> repository, IMapper mapper)
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
