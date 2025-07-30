using Nexus.Communication.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Application.UseCases.Reservation.GetByID
{
    public interface IGetByIdReservationUseCase
    {
        public Task<ResponseReservationJson?> ExecuteGetByIdAsync(int id);
    }
}
