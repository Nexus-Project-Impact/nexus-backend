using Nexus.Communication.Requests;
using Nexus.Communication.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Application.UseCases.Review.Register
{
    public interface IRegisterReviewUseCase
    {
        public Task<ResponseRegisteredReviewJson> Execute(RequestRegisterReviewJson request, string userId);
    }
}
