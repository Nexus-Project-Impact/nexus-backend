using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nexus.Communication.Responses;

namespace Nexus.Application.UseCases.Packages.GetByDestination
{
    public interface IGetByDestinationPackageUseCase
    {
        public Task<IEnumerable<ResponsePackage?>> ExecuteGetByDestination(string destination);
    }
}
