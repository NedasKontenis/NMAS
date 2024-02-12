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

            int? accommodationPlaceId = source.AccommodationPlaceId == 0 ? null : source.AccommodationPlaceId;

            var illegalMigrantEntityDocument = new IllegalMigrantEntityDocument
            {
                AccommodationPlaceId = accommodationPlaceId,
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
                Id = source.Id,
                AccommodationPlaceId = source.AccommodationPlaceId,
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

        public static Repositories.Models.IllegalMigrantEntity.FilterIllegalMigrantEntity Map(this Contracts.IllegalMigrantEntity.FilterIllegalMigrantEntity source, FilterIllegalMigrantEntityPagination pagination)
        {
            if (source == null)
            {
                return null;
            }

            var filterIllegalMigrantEntity = new Repositories.Models.IllegalMigrantEntity.FilterIllegalMigrantEntity
            {
                Ids = source.Ids,
                AccommodationPlaceIds = source.AccommodationPlaceIds,
                PersonalIdentityCodes = source.PersonalIdentityCodes,
                FirstNames = source.FirstNames,
                LastNames = source.LastNames,
                Genders = source.Genders,
                OriginCountries = source.OriginCountries,
                Religions = source.Religions,
                Limit = pagination.Limit,
                Offset = pagination.Offset,
                OrderBy = (Repositories.Models.IllegalMigrantEntity.FilterIllegalMigrantEntityOrderBy)pagination.OrderBy,
                OrderDirection = (Repositories.Models.IllegalMigrantEntity.FilterIllegalMigrantEntityOrderDirection)pagination.OrderDirection
            };

            if (source.DateOfBirth != null)
            {
                filterIllegalMigrantEntity.DateOfBirth = new Repositories.Models.IllegalMigrantEntity.FilterDateOfBirth(
                    source.DateOfBirth.From,
                    source.DateOfBirth.To);
            }

            return filterIllegalMigrantEntity;
        }
    }
}
