using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nexus.Communication.Responses;

namespace Nexus.Application.UseCases.TravelPackages.GetAllActive
{
    public interface IGetAllActivePackageUseCase
    {
        public Task<IEnumerable<ResponsePackage>> ExecuteGetAllActive();
    }
}
