using Nexus.Communication.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Application.UseCases.Review
{
    public interface IReviewUseCase
    {
        public Task<IEnumerable<ResponseReviewJson>> Execute();

    }
}
