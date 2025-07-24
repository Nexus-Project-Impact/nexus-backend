using AutoMapper;
using Nexus.Communication.Responses;
using Nexus.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Application.UseCases.Reservation.GetByID
{
    public class GetByIdReservationUseCase
    {
        private readonly IRepository<Nexus.Domain.Entities.Reservation, int> _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetByIdReservationUseCase(IRepository<Nexus.Domain.Entities.Reservation, int> repository, IMapper mapper,
           IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseReservation?> GetByIdAsync(int id)
        {
            var packages = await _repository.GetByIdAsync(id);

            var packagesJson = _mapper.Map<ResponseReservation>(packages);

            await _unitOfWork.Commit();

            return packagesJson;
        }
    }
}
