using NMAS.WebApi.Contracts.AccommodationPlaceEntity;
using NMAS.WebApi.Contracts.AccomodationPlaceEntity;
using NMAS.WebApi.Contracts.Exceptions;
using NMAS.WebApi.Repositories.AccommodationPlaceEntity;
using NMAS.WebApi.Services.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NMAS.WebApi.Services.AccommodationPlaceEntityService
{
    public class AccommodationPlaceEntityService : IAccommodationPlaceEntityService
    {
        private readonly IAccommodationPlaceEntityRepository _accommodationPlaceEntityRepository;

        public AccommodationPlaceEntityService(IAccommodationPlaceEntityRepository accommodationPlaceEntityRepository)
        {
            _accommodationPlaceEntityRepository = accommodationPlaceEntityRepository;
        }

        public async Task<AccommodationPlaceEntityCreated> CreateAsync(CreateAccommodationPlaceEntity createAccommodationPlaceEntity)
        {
            var document = createAccommodationPlaceEntity.Map();
            var createdAccommodationPlaceEntityId = await _accommodationPlaceEntityRepository.CreateAsync(document);

            return new AccommodationPlaceEntityCreated
            {
                Id = createdAccommodationPlaceEntityId
            };
        }

        public async Task<AccommodationPlaceEntity> GetAsync(int id)
        {
            var accommodationPlaceEntityDocument = await _accommodationPlaceEntityRepository.GetAsync(id);

            if (accommodationPlaceEntityDocument == null)
            {
                throw new ResourceNotFoundException("Accommodation place entity not found");
            }

            return accommodationPlaceEntityDocument.Map();
        }

        public async Task UpdateAsync(int id, UpdateAccommodationPlaceEntity updateAccommodationPlaceEntity)
        {
            var accommodationPlaceEntityDocument = await _accommodationPlaceEntityRepository.GetAsync(id);

            if (accommodationPlaceEntityDocument == null)
            {
                throw new ResourceNotFoundException("Accommodation place entity not found");
            }

            await _accommodationPlaceEntityRepository.UpdateAsync(id, updateAccommodationPlaceEntity.Map());
        }

        public async Task DeleteAsync(int id)
        {
            var accommodationPlaceEntityDocument = await _accommodationPlaceEntityRepository.GetAsync(id);

            if (accommodationPlaceEntityDocument == null)
            {
                throw new ResourceNotFoundException("Accommodation place entity not found");
            }

            await _accommodationPlaceEntityRepository.DeleteAsync(id);
        }

        public async Task IncrementUsedAccommodationCapacity(int accommodationPlaceId)
        {
            var accommodationPlace = await _accommodationPlaceEntityRepository.GetAsync(accommodationPlaceId);

            if (accommodationPlace != null)
            {
                accommodationPlace.UsedAccommodationCapacity += 1;
                await _accommodationPlaceEntityRepository.UpdateAsync(accommodationPlaceId, accommodationPlace);
            }
        }

        public async Task DecrementUsedAccommodationCapacity(int accommodationPlaceId)
        {
            var accommodationPlace = await _accommodationPlaceEntityRepository.GetAsync(accommodationPlaceId);

            if (accommodationPlace != null)
            {
                accommodationPlace.UsedAccommodationCapacity -= 1;
                await _accommodationPlaceEntityRepository.UpdateAsync(accommodationPlaceId, accommodationPlace);
            }
        }

        public async Task<IEnumerable<AccommodationPlaceEntity>> GetAllAccommodationPlacesAsync()
        {
            var accommodationPlaceEntities = await _accommodationPlaceEntityRepository.GetAllAccommodationPlacesAsync();

            return accommodationPlaceEntities
                .Select(AccommodationPlaceEntityMapperExtension.Map)
                .ToList();
        }

        public async Task<IEnumerable<AccommodationPlaceEntity>> ListAsync(FilterAccommodationPlaceEntity filter, FilterAccommodationPlaceEntityPagination pagination)
        {
            var accommodationPlaceEntities = await _accommodationPlaceEntityRepository.ListAsync(filter.Map(pagination));

            if (!accommodationPlaceEntities.Any())
            {
                throw new ResourceNotFoundException("Accommodation place entities not found");
            }

            return accommodationPlaceEntities
                .Select(AccommodationPlaceEntityMapperExtension.Map)
                .ToList();
        }
    }
}
