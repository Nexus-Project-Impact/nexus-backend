using Nexus.Communication.Requests;
using Nexus.Communication.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Application.UseCases.User.Auth
{
    public interface IAuthUserUseCase
    {
        public Task<ResponseLoginUserJson> Execute(RequestLoginUserJson request);
        public Task<ResponseForgotPassword> Execute(RequestForgotPassword request);

        public Task<ResponseMessage> ChangePassword(Guid userId, RequestChangePassword request);
        public Task<bool> ResetPassword(RequestResetPassword request);
        public Task Logout();
    }
}
