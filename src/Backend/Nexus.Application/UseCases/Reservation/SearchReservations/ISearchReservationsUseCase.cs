using Nexus.Communication.Responses;

namespace Nexus.Application.UseCases.Reservation.SearchReservations
{
    public interface ISearchReservationsUseCase
    {
        Task<IEnumerable<ResponseReservationAdminJson>> Execute(string userName, string userCpf);
    }
}