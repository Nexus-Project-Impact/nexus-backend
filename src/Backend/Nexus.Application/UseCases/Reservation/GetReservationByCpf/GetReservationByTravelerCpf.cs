using AutoMapper;
using Nexus.Communication.Responses;
using Nexus.Domain.Repositories;
using Nexus.Domain.Repositories.Reservation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Nexus.Application.UseCases.Reservation.GetReservationByCpf
{
    public class GetReservationByTravelerCpf : IGetReservationByTravelerCpf
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetReservationByTravelerCpf(IReservationRepository reservationRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _reservationRepository = reservationRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        } 

        public async Task<IEnumerable<ResponseReservationJson>> ExecuteGetReservationByTravelerCpfAsync(string Cpf)
        {
            var reservations = await _reservationRepository.GetReservationByCpfAsync(Cpf);

            var reservationsJson = _mapper.Map<IEnumerable<ResponseReservationJson>>(reservations);

            await _unitOfWork.Commit();
            
            return reservationsJson;
        }
    }
}
