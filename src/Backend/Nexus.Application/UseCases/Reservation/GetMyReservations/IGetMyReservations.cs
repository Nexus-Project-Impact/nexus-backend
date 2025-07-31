using Nexus.Communication.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Application.UseCases.Reservation.GetMyReservations
{
    public interface IGetMyReservations
    {
        public Task<IEnumerable<ResponseRegisteredReservationJson>> ExecuteGetMyReservationsAsync(string userId);
    }
}
