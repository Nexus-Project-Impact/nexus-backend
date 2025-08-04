using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nexus.Communication.Requests;
using Nexus.Communication.Responses;

namespace Nexus.Application.UseCases.Packages.Update
{
    public interface IUpdatePackageUseCase
    {
        public Task<ResponsePackage?> ExecuteUpdate(int id, RequestUpdatePackage request);
    }
}
