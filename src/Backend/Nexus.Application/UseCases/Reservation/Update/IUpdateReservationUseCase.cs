using Nexus.Communication.Requests;
using Nexus.Communication.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Application.UseCases.Reservation.Update
{
    public interface IUpdateReservationUseCase
    {
        public Task<ResponseRegisteredReservationJson?> ExecuteUpdateAsync(int id, RequestRegisterReservationJson requestReservation);
    }
}
