using AutoMapper;
using Nexus.Communication.Responses;
using Nexus.Domain.Repositories;
using Nexus.Domain.Repositories.Reservation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Application.UseCases.Reservation.GetMyReservations
{
    public class GetMyReservations : IGetMyReservations
    {

        private readonly IReservationRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetMyReservations(IReservationRepository repository, IMapper mapper,
           IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ResponseReservationJson>> ExecuteGetMyReservationsAsync(string userId)
        {
            var reservations = await _repository.GetMyReservationsAsync(userId);

            var reservationsJson = _mapper.Map<IEnumerable<ResponseReservationJson>>(reservations);

            await _unitOfWork.Commit();

            return reservationsJson;
        }
    }
}
