using AutoMapper;
using Nexus.Communication.Requests;
using Nexus.Communication.Responses;
using Nexus.Domain.DTOs;
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
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

            CreateMap<RequestCreatePackage, TravelPackage>();
            CreateMap<RequestUpdatePackage, TravelPackage>();

            CreateMap<RequestTravelers, Travelers>();

            CreateMap<PaymentDto, Payment>();
            //    .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<Payment, PaymentDto>();

            CreateMap<PackageDto, TravelPackage>();
            CreateMap<TravelPackage, PackageDto>();

            CreateMap<RequestRegisterReservationJson, Reservation>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.TravelPackageId, opt => opt.MapFrom(src => src.TravelPackageId))
                .ForMember(dest => dest.Traveler, opt => opt.MapFrom(src => src.Traveler));
            CreateMap<RequestUpdateReservationJson, Reservation>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.TravelPackageId, opt => opt.MapFrom(src => src.TravelPackageId))
                .ForMember(dest => dest.ReservationNumber, opt => opt.MapFrom(src => src.ReservationNumber))
                .ForMember(dest => dest.ReservationDate, opt => opt.MapFrom(src => src.ReservationDate))
                .ForMember(dest => dest.Traveler, opt => opt.MapFrom(src => src.Traveler));

            CreateMap<RequestRegisterReviewJson, Review>();
        }

        private void DomainToResponse()
        {
            CreateMap<Review, ResponseReviewJson>();
            CreateMap<Review, ResponseRegisteredReviewJson>();

            CreateMap<TravelPackage, ResponseCreatedPackage>();
            CreateMap<TravelPackage, ResponsePackage>();

            CreateMap<Reservation, ResponseReservationJson>()
                .ForMember(dest => dest.TravelPackageDestination, opt => opt.MapFrom(src => src.TravelPackage != null ? src.TravelPackage.Destination : null))
                .ForMember(dest => dest.TravelPackageImageUrl, opt => opt.MapFrom(src => src.TravelPackage != null ? src.TravelPackage.ImageUrl : null));

            CreateMap<Travelers, ResponseTravelers>();
        }
    }
}
