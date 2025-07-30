using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nexus.Communication.Responses;

namespace Nexus.Application.UseCases.Packages.GetByValue
{
    public interface IGetByValuePackageUseCase
    {
        public Task<IEnumerable<ResponsePackage?>> ExecuteGetByValue(double minValue, double maxValue);
    }
}
