﻿using NMAS.WebApi.Contracts.AccommodationPlaceEntity;
using NMAS.WebApi.Contracts.AccomodationPlaceEntity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NMAS.WebApi.Services.AccommodationPlaceEntityService
{
    public interface IAccommodationPlaceEntityService
    {
        Task<AccommodationPlaceEntityCreated> CreateAsync(CreateAccommodationPlaceEntity createAccommodationPlaceEntity);

        Task<AccommodationPlaceEntity> GetAsync(int id);

        Task UpdateAsync(int id, UpdateAccommodationPlaceEntity updateAccommodationPlaceEntity);

        Task DeleteAsync(int id);

        Task IncrementUsedAccommodationCapacity(int accommodationPlaceId);

        Task DecrementUsedAccommodationCapacity(int accommodationPlaceId);

        Task<IEnumerable<AccommodationPlaceEntity>> GetAllAccommodationPlacesAsync();

        Task<IEnumerable<AccommodationPlaceEntity>> ListAsync(FilterAccommodationPlaceEntity filter, FilterAccommodationPlaceEntityPagination pagination);
    }
}
