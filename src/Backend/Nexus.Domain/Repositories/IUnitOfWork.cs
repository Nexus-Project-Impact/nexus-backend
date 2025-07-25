using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Domain.Repositories
{
    public interface IUnitOfWork
    {
        public Task Commit();
    }
}
