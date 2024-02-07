using NMAS.WebApi.Contracts.IllegalMigrantEntity;
using NMAS.WebApi.Repositories.Models.IllegalMigrantEntity;

namespace NMAS.WebApi.Services.Extensions
{
    public static class IllegalMigrantEntityMapperExtension
    {
        public static IllegalMigrantEntityDocument Map(this ModifiableIllegalMigrantEntity source)
        {
            if (source == null)
            {
                return null;
            }

            int? accommodationPlaceId = source.AccommodationPlaceID == 0 ? null : source.AccommodationPlaceID;

            var illegalMigrantEntityDocument = new IllegalMigrantEntityDocument
            {
                AccommodationPlaceID = accommodationPlaceId,
                PersonalIdentityCode = source.PersonalIdentityCode,
                FirstName = source.FirstName,
                MiddleName = source.MiddleName,
                LastName = source.LastName,
                Gender = source.Gender,
                DateOfBirth = source.DateOfBirth,
                OriginCountry = source.OriginCountry,
                Religion = source.Religion
            };

            return illegalMigrantEntityDocument;
        }

        public static Contracts.IllegalMigrantEntity.IllegalMigrantEntity Map(this IllegalMigrantEntityDocument source)
        {
            if (source == null)
            {
                return null;
            }

            return new Contracts.IllegalMigrantEntity.IllegalMigrantEntity
            {
                AccommodationPlaceID = source.AccommodationPlaceID,
                PersonalIdentityCode = source.PersonalIdentityCode,
                FirstName = source.FirstName,
                MiddleName = source.MiddleName,
                LastName = source.LastName,
                Gender = source.Gender,
                DateOfBirth = source.DateOfBirth,
                OriginCountry = source.OriginCountry,
                Religion = source.Religion
            };
        }
    }
}
