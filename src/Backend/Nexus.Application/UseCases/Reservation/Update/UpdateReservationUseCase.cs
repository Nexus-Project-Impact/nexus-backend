using AutoMapper;
using Nexus.Communication.Requests;
using Nexus.Communication.Responses;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Application.UseCases.Reservation.Update
{
    public class UpdateReservationUseCase : IUpdateReservationUseCase
    {
        private readonly IRepository<Nexus.Domain.Entities.Reservation, int> _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateReservationUseCase(IRepository<Nexus.Domain.Entities.Reservation, int> repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseRegisteredReservationJson?> ExecuteUpdateAsync(int id, RequestRegisterReservationJson register)
        {
            var package = await _repository.GetByIdAsync(id);

            if (package == null)
            {
                return null;
            }

            _mapper.Map(register, package);

            await _repository.UpdateAsync(package);

            var packagesJson = _mapper.Map<ResponseRegisteredReservationJson>(package);

            await _unitOfWork.Commit();

            return packagesJson;
        }
    }
}
