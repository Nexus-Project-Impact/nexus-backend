using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Nexus.Communication.Responses;
using Nexus.Domain.Repositories;
using Nexus.Domain.Repositories.Reservation;

namespace Nexus.Application.UseCases.Reservation.GetAll
{
    public class GetAllReservationUseCase : IGetAllReservantionUseCase
    {

        private readonly IReservationRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetAllReservationUseCase(IReservationRepository repository, IMapper mapper,
           IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ResponseReservationJson>> ExecuteGetAllAsync()
        {
            var reservations = await _repository.GetAllAsync();

            var reservationsJson = _mapper.Map<IEnumerable<ResponseReservationJson>>(reservations);

            await _unitOfWork.Commit();

            return reservationsJson;
        }    
    }
}

