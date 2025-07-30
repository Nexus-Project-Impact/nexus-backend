using AutoMapper;
using global::Nexus.Communication.Responses;
using global::Nexus.Domain.Repositories.Reservation;
using Nexus.Domain.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nexus.Application.UseCases.Reservation.GetBytravelerName
{
    public class GetReservationByTravelerName : IGetReservationByTravelerName
    {
       private readonly IReservationRepository _reservationRepository;
       private readonly IMapper _mapper;
       private readonly IUnitOfWork _unitOfWork;
       public GetReservationByTravelerName(IReservationRepository reservationRepository, IMapper mapper, IUnitOfWork unitOfWork)
       {
           _reservationRepository = reservationRepository;
           _mapper = mapper;
           _unitOfWork = unitOfWork;
       }

       public async Task<IEnumerable<ResponseRegisteredReservationJson>> ExecuteGetReservationByTravelerNameAsync(string travelerName)
       {
          var reservations = await _reservationRepository.GetReservationByTravelerNameAsync(travelerName);

          var reservationsJson = _mapper.Map<IEnumerable<ResponseRegisteredReservationJson>>(reservations);

          await _unitOfWork.Commit();

          return reservationsJson; 
       }
    }
}

