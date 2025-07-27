using Nexus.Communication.Requests;
using Nexus.Communication.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Application.UseCases.Review.Moderate
{
    public interface IModerateReviewUseCase
    {
        public Task<ResponseModeratedReviewJson> Execute(RequestModerateReviewJson request);
    }
}
