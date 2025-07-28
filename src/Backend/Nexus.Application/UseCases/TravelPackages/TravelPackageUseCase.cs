using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Nexus.Communication.Requests;
using Nexus.Communication.Responses;
using Nexus.Domain.Repositories;
using Nexus.Infrastructure.DataAccess.Repositories;
using Nexus.Domain.Entities;
using Nexus.Application.UseCases.TravelPackage;
using Microsoft.AspNetCore.Mvc;

namespace Nexus.Application.UseCases.TravelPackages
{
    public class TravelPackageUseCase : ITravelPackageUseCase
    {
        private readonly IRepository<TravelPackageEntity, int> _repository;
        private readonly IMapper _mapper;

        public TravelPackageUseCase(IRepository<TravelPackageEntity, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ResponseTravelPackage>> GetAllAsync()
        {
            var packages = await _repository.GetAllAsync();

            var packagesJson = _mapper.Map<IEnumerable<ResponseTravelPackage>>(packages);

            return packagesJson;
        }
        public async Task<ResponseTravelPackage?> GetByIdAsync(int id)
        {
            var packages = await _repository.GetByIdAsync(id);

            var packagesJson = _mapper.Map<ResponseTravelPackage>(packages);

            return packagesJson;
        }

        public async Task<ResponseTravelPackage> AddAsync(RequestTravelPackage register)
        {
            var newPackage = _mapper.Map<TravelPackageEntity>(register);

             await _repository.AddAsync(newPackage);

            var packagesJson = _mapper.Map<ResponseTravelPackage>(newPackage);

            return packagesJson;
        }


        public async Task<ResponseTravelPackage?> UpdateAsync(int id, RequestTravelPackage register)
        {
            var package = await _repository.GetByIdAsync(id);

            if (package == null)
            {
                return null;
            }

            _mapper.Map(register, package);

            await _repository.UpdateAsync(package);

            var packagesJson = _mapper.Map<ResponseTravelPackage>(package);

            return packagesJson;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var package = await _repository.GetByIdAsync(id);
            if(package == null)
            {
                return false;
            }

            await _repository.DeleteAsync(id);

            return true;
        }
            

     




        //Task<ResponseTravelPackage> ITravelPackageUseCase.AddAsync(RequestTravelPackage requestTravelPackage)
        //{
        //    throw new NotImplementedException();
        //}
        //Task<IEnumerable<ResponseTravelPackage>> ITravelPackageUseCase.GetAllAsync()
        //{
        //    throw new NotImplementedException();
        //}
        //Task<ResponseTravelPackage?> ITravelPackageUseCase.GetByIdAsync(int id)
        //{
        //    throw new NotImplementedException();
        //}
       
        //Task ITravelPackageUseCase.UpdateAsync(int id, RequestTravelPackage requestTravelPackage)
        //{
        //    throw new NotImplementedException();
        //}

        //Task ITravelPackageUseCase.DeleteAsync(int id)
        //{
        //    throw new NotImplementedException();
        //}

    }
}
