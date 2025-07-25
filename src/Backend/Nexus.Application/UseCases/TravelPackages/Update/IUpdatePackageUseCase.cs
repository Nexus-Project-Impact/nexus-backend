using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nexus.Communication.Requests;
using Nexus.Communication.Responses;

namespace Nexus.Application.UseCases.TravelPackages.Update
{
    public interface IUpdatePackageUseCase
    {
        public Task<ResponseRegisteredPackageJson?> ExecuteUpdate(int id, RequestUpdatePackageJson request);
    }
}
