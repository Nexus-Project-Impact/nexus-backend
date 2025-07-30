using Nexus.Communication.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Application.UseCases.Reservation.GetBytravelerName
{
    public interface IGetReservationByTravelerName
    {
        public Task<IEnumerable<ResponseRegisteredReservationJson>> ExecuteGetReservationByTravelerNameAsync(string travelerName);
    }
}
