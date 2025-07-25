using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nexus.Communication.Responses;

namespace Nexus.Application.UseCases.TravelPackages.GetId
{
    public interface IGetByIdPackageUseCase
    {
        public Task<ResponsePackageJson?> ExecuteGetById(int id);
    }
}
