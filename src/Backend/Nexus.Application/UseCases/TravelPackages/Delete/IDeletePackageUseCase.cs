using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Application.UseCases.TravelPackages.Delete
{
    public interface IDeletePackageUseCase
    {
        public Task<bool> ExecuteDelete(int id);
    }
}
