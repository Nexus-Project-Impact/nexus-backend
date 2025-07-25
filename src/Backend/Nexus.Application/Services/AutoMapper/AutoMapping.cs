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
            CreateMap<RequestRegisterPackageJson, TravelPackageEntity>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Destination, opt => opt.MapFrom(src => src.Destination))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
                .ForMember(dest => dest.DepartureDate, opt => opt.MapFrom(src => src.DepartureDate))
                .ForMember(dest => dest.ReturnDate, opt => opt.MapFrom(src => src.ReturnDate))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value));
            CreateMap<RequestUpdatePackageJson, TravelPackageEntity>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Destination, opt => opt.MapFrom(src => src.Destination))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
                .ForMember(dest => dest.DepartureDate, opt => opt.MapFrom(src => src.DepartureDate))
                .ForMember(dest => dest.ReturnDate, opt => opt.MapFrom(src => src.ReturnDate))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value));
        }

        private void DomainToResponse() 
        {
            CreateMap<Review, ResponseReviewJson>();
            CreateMap<TravelPackageEntity, ResponsePackageJson>();

        }
    }
}
