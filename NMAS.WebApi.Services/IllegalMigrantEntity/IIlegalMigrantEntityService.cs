using NMAS.WebApi.Contracts.Exceptions;
using NMAS.WebApi.Contracts.IllegalMigrantEntity;
using NMAS.WebApi.Repositories.IllegalMigrantEntity;
using NMAS.WebApi.Services.AccommodationPlaceEntityService;
using NMAS.WebApi.Services.Extensions;
using System.Linq;
using System.Threading.Tasks;

namespace NMAS.WebApi.Services.IllegalMigrantEntity
{
    public class IllegalMigrantEntityService : IIllegalMigrantEntityService
    {
        private readonly IIllegalMigrantEntityRepository _illegalMigrantEntityRepository;
        private readonly IAccommodationPlaceEntityService _accommodationPlaceEntityService;

        public IllegalMigrantEntityService(IIllegalMigrantEntityRepository illegalMigrantEntityRepository,
            IAccommodationPlaceEntityService accommodationPlaceEntityService)
        {
            _illegalMigrantEntityRepository = illegalMigrantEntityRepository;
            _accommodationPlaceEntityService = accommodationPlaceEntityService;
        }

        public async Task<IllegalMigrantEntityCreated> CreateAsync(CreateIllegalMigrantEntity createIllegalMigrantEntity)
        {
            var document = createIllegalMigrantEntity.Map();
            var createdIllegalMigrantEntityId = await _illegalMigrantEntityRepository.CreateAsync(document);

            if (createIllegalMigrantEntity.AccommodationPlaceID.HasValue)
            {
                await _accommodationPlaceEntityService.IncrementUsedAccommodationCapacity(createIllegalMigrantEntity.AccommodationPlaceID.Value);
            }

            return new IllegalMigrantEntityCreated
            {
                Id = createdIllegalMigrantEntityId
            };
        }

        public async Task<Contracts.IllegalMigrantEntity.IllegalMigrantEntity> GetAsync(int id)
        {
            var illegalMigrantEntityDocument = await _illegalMigrantEntityRepository.GetAsync(id);

            if (illegalMigrantEntityDocument == null)
            {
                throw new ResourceNotFoundException("Illegal migrant entity not found");
            }

            return illegalMigrantEntityDocument.Map();
        }

        public async Task UpdateAsync(int id, UpdateIllegalMigrantEntity updateIllegalMigrantEntity)
        {
            var illegalMigrantEntityDocument = await _illegalMigrantEntityRepository.GetAsync(id);

            if (illegalMigrantEntityDocument == null)
            {
                throw new ResourceNotFoundException("Illegal migrant entity not found");
            }

            if (illegalMigrantEntityDocument.AccommodationPlaceID.HasValue &&
                (!updateIllegalMigrantEntity.AccommodationPlaceID.HasValue ||
                 updateIllegalMigrantEntity.AccommodationPlaceID.Value != illegalMigrantEntityDocument.AccommodationPlaceID.Value))
            {
                await _accommodationPlaceEntityService.DecrementUsedAccommodationCapacity(illegalMigrantEntityDocument.AccommodationPlaceID.Value);
            }

            if (updateIllegalMigrantEntity.AccommodationPlaceID.HasValue &&
                (!illegalMigrantEntityDocument.AccommodationPlaceID.HasValue ||
                 updateIllegalMigrantEntity.AccommodationPlaceID.Value != illegalMigrantEntityDocument.AccommodationPlaceID.Value))
            {
                await _accommodationPlaceEntityService.IncrementUsedAccommodationCapacity(updateIllegalMigrantEntity.AccommodationPlaceID.Value);
            }

            await _illegalMigrantEntityRepository.UpdateAsync(id, updateIllegalMigrantEntity.Map());
        }

        public async Task DeleteAsync(int id)
        {
            var illegalMigrantEntityDocument = await _illegalMigrantEntityRepository.GetAsync(id);

            if (illegalMigrantEntityDocument == null)
            {
                throw new ResourceNotFoundException("Illegal migrant entity not found");
            }

            if (illegalMigrantEntityDocument.AccommodationPlaceID.HasValue)
            {
                await _accommodationPlaceEntityService.DecrementUsedAccommodationCapacity(illegalMigrantEntityDocument.AccommodationPlaceID.Value);
            }

            await _illegalMigrantEntityRepository.DeleteAsync(id);
        }

        public async Task AssignAsync(int id)
        {
            var illegalMigrantEntityDocument = await _illegalMigrantEntityRepository.GetAsync(id);

            if (illegalMigrantEntityDocument == null)
            {
                throw new ResourceNotFoundException("Illegal migrant entity not found");
            }

            if (illegalMigrantEntityDocument.AccommodationPlaceID.HasValue)
            {
                throw new BadRequestException("Illegal migrant is already assigned to an accommodation place");
            }

            var accommodationPlaces = await _accommodationPlaceEntityService.GetAllAccommodationPlacesAsync();

            var availableAccommodationPlace = accommodationPlaces
                .FirstOrDefault(place => place.UsedAccommodationCapacity < place.AccommodationCapacity);

            if (availableAccommodationPlace == null)
            {
                throw new BadRequestException("No accommodation places with available capacity");
            }

            illegalMigrantEntityDocument.AccommodationPlaceID = availableAccommodationPlace.Id;
            await _illegalMigrantEntityRepository.UpdateAsync(id, illegalMigrantEntityDocument);

            availableAccommodationPlace.UsedAccommodationCapacity += 1;
            await _accommodationPlaceEntityService.IncrementUsedAccommodationCapacity(availableAccommodationPlace.Id);
        }
    }
}
