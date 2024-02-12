using NMAS.WebApi.Contracts.AccommodationPlaceEntity;
using NMAS.WebApi.Repositories.Models.AccommodationPlaceEntity;

namespace NMAS.WebApi.Services.Extensions
{
    public static class AccommodationPlaceEntityMapperExtension
    {
        public static AccommodationPlaceEntityDocument Map(this ModifiableAccommodationPlaceEntity source)
        {
            if (source == null)
            {
                return null;
            }

            var accommodationEntityDocument = new AccommodationPlaceEntityDocument
            {
                WorkerId = source.WorkerId,
                PlaceName = source.PlaceName,
                Adress = source.Adress,
                AccommodationCapacity = source.AccommodationCapacity,
                UsedAccommodationCapacity = source.UsedAccommodationCapacity,
                CompanyCode = source.CompanyCode,
                ContactPhone = source.ContactPhone
            };

            return accommodationEntityDocument;
        }

        public static AccommodationPlaceEntity Map(this AccommodationPlaceEntityDocument source)
        {
            if (source == null)
            {
                return null;
            }

            return new AccommodationPlaceEntity
            {
                Id = source.Id,
                WorkerId = source.WorkerId,
                PlaceName = source.PlaceName,
                Adress = source.Adress,
                AccommodationCapacity = source.AccommodationCapacity,
                UsedAccommodationCapacity = source.UsedAccommodationCapacity,
                CompanyCode = source.CompanyCode,
                ContactPhone = source.ContactPhone
            };
        }

        public static Repositories.Models.AccommodationPlaceEntity.FilterAccommodationPlaceEntity Map(this Contracts.AccommodationPlaceEntity.FilterAccommodationPlaceEntity source, FilterAccommodationPlaceEntityPagination pagination)
        {
            if (source == null)
            {
                return null;
            }

            var filterAccommodationPlaceEntity = new Repositories.Models.AccommodationPlaceEntity.FilterAccommodationPlaceEntity
            {
                Ids = source.Ids,
                PlaceNames = source.PlaceNames,
                WorkerIds = source.WorkerIds,
                Adresses = source.Adresses,
                CompanyCodes = source.CompanyCodes,
                Limit = pagination.Limit,
                Offset = pagination.Offset,
                OrderBy = (Repositories.Models.AccommodationPlaceEntity.FilterAccommodationPlaceEntityOrderBy)pagination.OrderBy,
                OrderDirection = (Repositories.Models.AccommodationPlaceEntity.FilterAccommodationPlaceEntityOrderDirection)pagination.OrderDirection
            };

            return filterAccommodationPlaceEntity;
        }
    }
}
