using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nexus.Communication.Responses;

namespace Nexus.Application.UseCases.Packages.GetById
{
    public interface IGetByIdPackageUseCase
    {
        public Task<ResponsePackage?> ExecuteGetById(int id); 
    }
}
