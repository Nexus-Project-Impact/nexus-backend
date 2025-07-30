using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nexus.Communication.Requests;
using Nexus.Communication.Responses;

namespace Nexus.Application.UseCases.Packages.Create
{
    public interface ICreatePackageUseCase
    {
        public Task<ResponseCreatedPackage> ExecuteCreate(RequestCreatePackage request);
    }
}
