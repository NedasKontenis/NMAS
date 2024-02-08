using NMAS.WebApi.Contracts.AccommodationPlaceEntity;
using NMAS.WebApi.Contracts.AccomodationPlaceEntity;
using NMAS.WebApi.Contracts.Exceptions;
using NMAS.WebApi.Repositories.AccommodationPlaceEntity;
using NMAS.WebApi.Services.Extensions;
using System.Threading.Tasks;

namespace NMAS.WebApi.Services.AccommodationPlaceEntityService
{
    public class AccommodationPlaceEntityService : IAccommodationPlaceEntityService
    {
        private readonly IAccommodationPlaceEntityRepository _accommodationEntityRepository;

        public AccommodationPlaceEntityService(IAccommodationPlaceEntityRepository accommodationEntityRepository)
        {
            _accommodationEntityRepository = accommodationEntityRepository;
        }

        public async Task<AccommodationPlaceEntityCreated> CreateAsync(CreateAccommodationPlaceEntity createAccommodationPlaceEntity)
        {
            var document = createAccommodationPlaceEntity.Map();
            var createdAccommodationPlaceEntityId = await _accommodationEntityRepository.CreateAsync(document);

            return new AccommodationPlaceEntityCreated
            {
                Id = createdAccommodationPlaceEntityId
            };
        }

        public async Task<AccommodationPlaceEntity> GetAsync(int id)
        {
            var accommodationPlaceEntityDocument = await _accommodationEntityRepository.GetAsync(id);

            if (accommodationPlaceEntityDocument == null)
            {
                throw new ResourceNotFoundException("Accommodation place entity not found");
            }

            return accommodationPlaceEntityDocument.Map();
        }

        public async Task UpdateAsync(int id, UpdateAccommodationPlaceEntity updateAccommodationPlaceEntity)
        {
            var accommodationPlaceEntityDocument = await _accommodationEntityRepository.GetAsync(id);

            if (accommodationPlaceEntityDocument == null)
            {
                throw new ResourceNotFoundException("Accommodation place entity not found");
            }

            await _accommodationEntityRepository.UpdateAsync(id, updateAccommodationPlaceEntity.Map());
        }

        public async Task DeleteAsync(int id)
        {
            var accommodationPlaceEntityDocument = await _accommodationEntityRepository.GetAsync(id);

            if (accommodationPlaceEntityDocument == null)
            {
                throw new ResourceNotFoundException("Accommodation place entity not found");
            }

            await _accommodationEntityRepository.DeleteAsync(id);
        }
    }
}
