using Nexus.Communication.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Application.UseCases.Reservation.GetReservationByCpf
{
    public interface IGetReservationByTravelerCpf
    {
        public Task<IEnumerable<ResponseRegisteredReservationJson>> ExecuteGetReservationByTravelerCpfAsync(string Cpf);
    }
}
