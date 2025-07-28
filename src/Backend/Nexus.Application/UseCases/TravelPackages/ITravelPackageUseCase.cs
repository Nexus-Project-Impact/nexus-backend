using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nexus.Communication.Requests;
using Nexus.Communication.Responses;


namespace Nexus.Application.UseCases.TravelPackage
{
    public interface ITravelPackageUseCase
    {
        public Task<IEnumerable<ResponseTravelPackage>> GetAllAsync();
        public Task<ResponseTravelPackage?> GetByIdAsync(int id);
        public Task<ResponseTravelPackage> AddAsync(RequestTravelPackage requestTravelPackage);
        public Task<ResponseTravelPackage?> UpdateAsync(int id, RequestTravelPackage requestTravelPackage);
        public Task<bool> DeleteAsync(int id);
    }
}
