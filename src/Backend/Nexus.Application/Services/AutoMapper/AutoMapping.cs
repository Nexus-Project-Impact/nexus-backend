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
            // Novo mapeamento para PackageDto -> TravelPackage
            CreateMap<PackageDto, TravelPackage>();


            CreateMap<RequestTravelers, Travelers>();

            CreateMap<PaymentDto, Payment>();
            //    .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<Payment, PaymentDto>();

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


        }

        private void DomainToResponse() 
        {
            CreateMap<Review, ResponseReviewJson>();

            CreateMap<TravelPackage, ResponseCreatedPackage>();
            CreateMap<TravelPackage, ResponsePackage>();

            CreateMap<Reservation, ResponseReservationJson>();
            CreateMap<Travelers, ResponseTravelers>();


        }
    }
}
