using NMAS.WebApi.Repositories.Models.AccommodationPlaceEntity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NMAS.WebApi.Repositories.AccommodationPlaceEntity
{
    public interface IAccommodationPlaceEntityRepository
    {
        Task<int> CreateAsync(AccommodationPlaceEntityDocument document);

        Task<AccommodationPlaceEntityDocument> GetAsync(int id);

        Task UpdateAsync(int id, AccommodationPlaceEntityDocument document);

        Task DeleteAsync(int id);

        Task<IEnumerable<AccommodationPlaceEntityDocument>> GetAllAccommodationPlacesAsync();

        Task<IEnumerable<AccommodationPlaceEntityDocument>> ListAsync(FilterAccommodationPlaceEntity filter);
    }
}