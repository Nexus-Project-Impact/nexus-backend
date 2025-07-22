using AutoMapper;
using Nexus.Communication.Requests;
using Nexus.Domain.Entities;

namespace Nexus.Application.Services.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping() 
        {
            RequestToDomain();
            DomainToResponse();
        }

        private void RequestToDomain() 
        {
            CreateMap<RequestRegisterUserJson, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
                
        }

        private void DomainToResponse() 
        {

        }
    }
}
