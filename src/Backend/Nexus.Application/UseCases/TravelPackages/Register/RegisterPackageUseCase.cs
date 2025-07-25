using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Identity.Client;
using Nexus.Communication.Requests;
using Nexus.Communication.Responses;
using Nexus.Domain.Repositories;

namespace Nexus.Application.UseCases.TravelPackages.Create
{
    public class RegisterPackageUseCase : IRegisterPackageUseCase
    {
        private readonly IRepository<TravelPackageEntity, int> _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterPackageUseCase(IRepository<TravelPackageEntity, int> repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseRegisteredPackageJson> Execute(RequestRegisterPackageJson request)
        {
            var package = _mapper.Map<TravelPackageEntity>(request);

            await _repository.AddAsync(package);

            await _unitOfWork.Commit();

            return new ResponseRegisteredPackageJson
            {
                Title = package.Title
            };
        }

        //public async Task Validate(RequestCreatedTravelPackage request)
        //{
            


        //}








    }
}
