using AutoMapper;
using Nexus.Communication.Requests;
using Nexus.Communication.Responses;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories;
using Nexus.Domain.Repositories.Reservation;
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
        private readonly IReservationRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateReservationUseCase(IReservationRepository repository, IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Execute(RequestRegisterReservationJson request)
        {
            var reservation = _mapper.Map<Nexus.Domain.Entities.Reservation>(request);

            await _repository.AddAsync(reservation);

            await _unitOfWork.Commit();


            return reservation.Id;

            //return new ResponseRegisteredReservationJson
            //{
            //    Message = "Reserva realizada com sucesso!",
            //};



        }
    }
}
