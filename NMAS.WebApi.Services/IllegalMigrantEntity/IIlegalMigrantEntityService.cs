﻿using NMAS.WebApi.Contracts.Enums;
using NMAS.WebApi.Contracts.Exceptions;
using NMAS.WebApi.Contracts.IllegalMigrantEntity;
using NMAS.WebApi.Repositories.IllegalMigrantEntity;
using NMAS.WebApi.Services.AccommodationPlaceEntityService;
using NMAS.WebApi.Services.Extensions;
using System.Collections.Generic;
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

            if (createIllegalMigrantEntity.AccommodationPlaceId.HasValue)
            {
                await _accommodationPlaceEntityService.IncrementUsedAccommodationCapacity(createIllegalMigrantEntity.AccommodationPlaceId.Value);
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

            if (illegalMigrantEntityDocument.AccommodationPlaceId.HasValue &&
                (!updateIllegalMigrantEntity.AccommodationPlaceId.HasValue ||
                 updateIllegalMigrantEntity.AccommodationPlaceId.Value != illegalMigrantEntityDocument.AccommodationPlaceId.Value))
            {
                await _accommodationPlaceEntityService.DecrementUsedAccommodationCapacity(illegalMigrantEntityDocument.AccommodationPlaceId.Value);
            }

            if (updateIllegalMigrantEntity.AccommodationPlaceId.HasValue &&
                (!illegalMigrantEntityDocument.AccommodationPlaceId.HasValue ||
                 updateIllegalMigrantEntity.AccommodationPlaceId.Value != illegalMigrantEntityDocument.AccommodationPlaceId.Value))
            {
                await _accommodationPlaceEntityService.IncrementUsedAccommodationCapacity(updateIllegalMigrantEntity.AccommodationPlaceId.Value);
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

            if (illegalMigrantEntityDocument.AccommodationPlaceId.HasValue)
            {
                await _accommodationPlaceEntityService.DecrementUsedAccommodationCapacity(illegalMigrantEntityDocument.AccommodationPlaceId.Value);
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

            if (illegalMigrantEntityDocument.AccommodationPlaceId.HasValue)
            {
                throw new BadRequestException("Illegal migrant is already assigned to an accommodation place");
            }

            var allMigrants = await _illegalMigrantEntityRepository.GetAllAsync();

            var accommodationPlaces = await _accommodationPlaceEntityService.GetAllAccommodationPlacesAsync();
            int? availableAccommodationPlaceId = null;

            foreach (var place in accommodationPlaces.OrderBy(p => p.Id))
            {
                if (place.UsedAccommodationCapacity >= place.AccommodationCapacity)
                {
                    continue;
                }

                var migrantsInPlace = allMigrants.Where(m => m.AccommodationPlaceId == place.Id).ToList();

                bool isReligionCompatible = !migrantsInPlace.Any(m =>
                    (m.Religion == IllegalMigrantReligion.Sunni.ToString() && illegalMigrantEntityDocument.Religion == IllegalMigrantReligion.Shia.ToString()) ||
                    (m.Religion == IllegalMigrantReligion.Shia.ToString() && illegalMigrantEntityDocument.Religion == IllegalMigrantReligion.Sunni.ToString()));

                if (isReligionCompatible)
                {
                    availableAccommodationPlaceId = place.Id;
                    break;
                }
            }

            if (availableAccommodationPlaceId == null)
            {
                throw new BadRequestException("No suitable accommodation place found based on capacity and religious compatibility");
            }

            illegalMigrantEntityDocument.AccommodationPlaceId = availableAccommodationPlaceId;
            await _illegalMigrantEntityRepository.UpdateAsync(id, illegalMigrantEntityDocument);

            await _accommodationPlaceEntityService.IncrementUsedAccommodationCapacity((int)availableAccommodationPlaceId);
        }

        public async Task<IEnumerable<Contracts.IllegalMigrantEntity.IllegalMigrantEntity>> ListAsync(FilterIllegalMigrantEntity filter, FilterIllegalMigrantEntityPagination pagination)
        {
            var illegalMigrantEntities = await _illegalMigrantEntityRepository.ListAsync(filter.Map(pagination));

            if (!illegalMigrantEntities.Any())
            {
                throw new ResourceNotFoundException("Illegal migrant entities not found");
            }

            return illegalMigrantEntities
                .Select(IllegalMigrantEntityMapperExtension.Map)
                .ToList();
        }
    }
}
