using Nexus.Communication.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Application.UseCases.Review.GetAll
{
    public interface IGetAllReviewUseCase
    {
        public Task<IEnumerable<ResponseReviewJson>> ExecuteGetAll();
    }
}
