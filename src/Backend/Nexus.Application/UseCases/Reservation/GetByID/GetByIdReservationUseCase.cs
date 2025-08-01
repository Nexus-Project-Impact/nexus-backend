using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Nexus.Communication.Responses;
using Nexus.Domain.Repositories;
using Nexus.Domain.Repositories.Reservation;

namespace Nexus.Application.UseCases.Reservation.GetByID
{
    public class GetByIdReservationUseCase : IGetByIdReservationUseCase
    {
        private readonly IReservationRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetByIdReservationUseCase(IReservationRepository repository, IMapper mapper,
           IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseRegisteredReservationJson?> ExecuteGetByIdAsync(int id)
        {
            var reservation = await _repository.GetByIdAsync(id);

            var reservationJson = _mapper.Map<ResponseRegisteredReservationJson>(reservation);

            await _unitOfWork.Commit();

            return reservationJson;
        }
    }
}
