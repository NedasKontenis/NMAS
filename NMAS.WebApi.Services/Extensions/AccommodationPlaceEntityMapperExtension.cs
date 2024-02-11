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
                WorkerID = source.WorkerID,
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
                ID = source.ID,
                WorkerID = source.WorkerID,
                PlaceName = source.PlaceName,
                Adress = source.Adress,
                AccommodationCapacity = source.AccommodationCapacity,
                UsedAccommodationCapacity = source.UsedAccommodationCapacity,
                CompanyCode = source.CompanyCode,
                ContactPhone = source.ContactPhone
            };
        }
    }
}
