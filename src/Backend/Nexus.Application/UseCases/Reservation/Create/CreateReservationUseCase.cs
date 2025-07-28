using AutoMapper;
using Nexus.Communication.Requests;
using Nexus.Communication.Responses;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories;
using Nexus.Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Nexus.Application.UseCases.Reservation.Create
{
    public class CreateReservationUseCase : ICreateReservationUseCase
    {
        private readonly IRepository<Nexus.Domain.Entities.Reservation, int> _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateReservationUseCase(IRepository<Nexus.Domain.Entities.Reservation, int> repository, IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseRegisteredReservationJson> Execute(RequestRegisterReservationJson request)
        {
            var reservation = _mapper.Map<Nexus.Domain.Entities.Reservation>(request);

            await _repository.AddAsync(reservation);

            await _unitOfWork.Commit();

            return new ResponseRegisteredReservationJson
            {
                ReservationNumber = reservation.ReservationNumber
            };
        }
    }
}
