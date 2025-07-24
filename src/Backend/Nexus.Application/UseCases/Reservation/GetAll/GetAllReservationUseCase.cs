using AutoMapper;
using Nexus.Communication.Responses;
using Nexus.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Application.UseCases.Reservation.GetAll
{
    public class GetAllReservationUseCase
    {

        private readonly IRepository<Nexus.Domain.Entities.Reservation, int> _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetAllReservationUseCase(IRepository<Nexus.Domain.Entities.Reservation, int> repository, IMapper mapper,
           IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ResponseTravelPackage>> GetAllAsync()
        {
            var packages = await _repository.GetAllAsync();

            var packagesJson = _mapper.Map<IEnumerable<ResponseTravelPackage>>(packages);

            await _unitOfWork.Commit();

            return packagesJson;
        }    
    }
}

