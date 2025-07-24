using AutoMapper;
using Nexus.Communication.Requests;
using Nexus.Communication.Responses;
using Nexus.Domain.Repositories;
using Nexus.Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Nexus.Application.UseCases.Reservation.Create
{
    public class CreateReservationUseCase
    {
        private readonly IRepository<Nexus.Domain.Entities.Reservation, int > _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateReservationUseCase(IRepository<Nexus.Domain.Entities.Reservation, int> repository, IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseReservation> AddAsync(RequestReservation register)
        {
            var newReservation = _mapper.Map<Nexus.Domain.Entities.Reservation>(register);

            await _repository.AddAsync(newReservation);

            var packagesJson = _mapper.Map<ResponseReservation>(newReservation);

            await _unitOfWork.Commit();

            return packagesJson;

        }
    }
}
