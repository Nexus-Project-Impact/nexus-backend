using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Nexus.Communication.Requests;
using Nexus.Communication.Responses;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories;
using Nexus.Domain.Repositories.Packages;

namespace Nexus.Application.UseCases.Packages.Create
{
    internal class CreatePackageUseCase : ICreatePackageUseCase
    {
        private readonly IPackageRepository<TravelPackage, int> _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreatePackageUseCase(IPackageRepository<TravelPackage, int> repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseCreatedPackage> ExecuteCreate(RequestCreatePackage request)
        {
            var package = _mapper.Map<TravelPackage>(request);

            await _repository.AddAsync(package);

            await _unitOfWork.Commit();

            return new ResponseCreatedPackage
            {
                Title = package.Title
            };
        }
    }
}
