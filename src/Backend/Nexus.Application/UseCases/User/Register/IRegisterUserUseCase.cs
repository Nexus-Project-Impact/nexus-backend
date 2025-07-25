using Nexus.Communication.Requests;
using Nexus.Communication.Responses;

namespace Nexus.Application.UseCases.User.Register
{
    public interface IRegisterUserUseCase
    {
        public Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request);
    }
}
