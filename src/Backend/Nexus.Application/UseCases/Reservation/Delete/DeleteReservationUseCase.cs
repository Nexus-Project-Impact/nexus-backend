using AutoMapper;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Application.UseCases.Reservation.Delete
{
    public class DeleteReservationUseCase
    {
        private readonly IRepository<Nexus.Domain.Entities.Reservation, int> _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteReservationUseCase(IRepository<Nexus.Domain.Entities.Reservation, int> repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var reservation = await _repository.GetByIdAsync(id);
            if (reservation == null)
            {
                return false;
            }

            await _repository.DeleteAsync(id);

            await _unitOfWork.Commit();

            return true;
        }
    }
}
