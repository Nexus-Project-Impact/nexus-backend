using Nexus.Domain.Entities;

namespace Nexus.Domain.Repositories.Packages
{
    public interface IPackageRepository<T, TId> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T?> GetByIdAsync(TId id);

        Task AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(TId id);

        Task<IEnumerable<TravelPackage>> GetByDepartureDateAsync(DateTime? initialDepartureDate, DateTime? finalDepartureDate);

        Task<IEnumerable<TravelPackage>> GetByValueAsync(double? minValue, double? maxValue);

        Task<IEnumerable<TravelPackage>> GetByDestinationAsync(string destination);
    }
}
