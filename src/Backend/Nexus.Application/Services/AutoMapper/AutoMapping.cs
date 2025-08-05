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

            CreateMap<RequestRegisterReviewJson, Review>();
        }

        private void DomainToResponse() 
        {
            CreateMap<Review, ResponseReviewJson>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.Name : null));
            
            CreateMap<Review, ResponseRegisteredReviewJson>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.Name : null));

            CreateMap<TravelPackage, ResponseCreatedPackage>();
            CreateMap<TravelPackage, ResponsePackage>();

            CreateMap<Reservation, ResponseReservationJson>()
                .ForMember(dest => dest.TravelPackageDestination, opt => opt.MapFrom(src => src.TravelPackage != null ? src.TravelPackage.Destination : null))
                .ForMember(dest => dest.TravelPackageImageUrl, opt => opt.MapFrom(src => src.TravelPackage != null ? src.TravelPackage.ImageUrl : null));

            CreateMap<Reservation, ResponseReservationAdminJson>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User!.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User!.Name))
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User!.Email))
                .ForMember(dest => dest.UserPhone, opt => opt.MapFrom(src => src.User!.Phone))
                .ForMember(dest => dest.UserDocument, opt => opt.MapFrom(src => src.User!.CPF))
                .ForMember(dest => dest.StatusPayment, opt => opt.MapFrom(src => src.Payment == null || string.IsNullOrEmpty(src.Payment.Status) ? "Pendente" : src.Payment.Status))
                .ForMember(dest => dest.TravelPackageName, opt => opt.MapFrom(src => src.TravelPackage!.Title))
                .ForMember(dest => dest.TravelPackageDestination, opt => opt.MapFrom(src => src.TravelPackage!.Destination))
                .ForMember(dest => dest.TotalValue, opt => opt.MapFrom(src => src.TravelPackage!.Value * src.Traveler.Count));

            CreateMap<Travelers, ResponseTravelers>();
        }
    }
}
