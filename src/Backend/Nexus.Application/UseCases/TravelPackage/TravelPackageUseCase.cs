using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nexus.Communication.Requests;
using Nexus.Communication.Responses;
using Nexus.Domain.Repositories;

namespace Nexus.Application.UseCases.TravelPackage
{
    public class TravelPackageUseCase : ITravelPackageUseCase
    {
        Task<ResponseTravelPackage> ITravelPackageUseCase.AddAsync(RequestTravelPackage requestTravelPackage)
        {
            throw new NotImplementedException();
        }
        Task<IEnumerable<ResponseTravelPackage>> ITravelPackageUseCase.GetAllAsync()
        {
            throw new NotImplementedException();
        }
        Task<ResponseTravelPackage?> ITravelPackageUseCase.GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
       
        Task ITravelPackageUseCase.UpdateAsync(int id, RequestTravelPackage requestTravelPackage)
        {
            throw new NotImplementedException();
        }

        Task ITravelPackageUseCase.DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

    }
}
