using AutoMapper;
using Nexus.Communication.Requests;
using Nexus.Communication.Responses;
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
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
            CreateMap<RequestCreatePackage, TravelPackage>();
            CreateMap<RequestUpdatePackage, TravelPackage>();
        }

        private void DomainToResponse() 
        {
            CreateMap<Review, ResponseReviewJson>();
            CreateMap<TravelPackage, ResponseCreatedPackage>();
            CreateMap<TravelPackage, ResponsePackage>();

        }
    }
}
