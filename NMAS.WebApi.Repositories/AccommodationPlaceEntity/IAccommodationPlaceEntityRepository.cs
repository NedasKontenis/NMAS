using NMAS.WebApi.Repositories.Models.AccommodationPlaceEntity;
using System.Threading.Tasks;

namespace NMAS.WebApi.Repositories.AccommodationPlaceEntity
{
    public interface IAccommodationPlaceEntityRepository
    {
        Task<int> CreateAsync(AccommodationPlaceEntityDocument document);

        Task<AccommodationPlaceEntityDocument> GetAsync(int id);

        Task UpdateAsync(int id, AccommodationPlaceEntityDocument document);

        Task DeleteAsync(int id);
    }
}
