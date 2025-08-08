using AutoMapper;
using Nexus.Communication.Responses;
using Nexus.Domain.Repositories.Reservation;
using Nexus.Domain.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nexus.Application.UseCases.Reservation.SearchReservations
{
    public class SearchReservationsUseCase : ISearchReservationsUseCase
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public SearchReservationsUseCase(IReservationRepository reservationRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _reservationRepository = reservationRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ResponseReservationAdminJson>> Execute(string userName, string userCpf)
        {
            var reservations = await _reservationRepository.SearchByUserAsync(userName, userCpf);

            var reservationsJson = _mapper.Map<IEnumerable<ResponseReservationAdminJson>>(reservations);

            return reservationsJson;
        }
    }
}