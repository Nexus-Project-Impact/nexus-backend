using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nexus.Communication.Responses;
using Nexus.Communication.Requests;

namespace Nexus.Application.UseCases.TravelPackages.Create
{
    public interface IRegisterPackageUseCase
    {
        public Task<ResponseRegisteredPackageJson> Execute(RequestRegisterPackageJson request);
    }
}
