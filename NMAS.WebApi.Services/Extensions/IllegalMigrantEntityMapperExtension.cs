using NMAS.WebApi.Repositories.Models.IllegalMigrantEntity;

namespace NMAS.WebApi.Services.Extensions
{
    public static class IllegalMigrantEntityMapperExtension
    {
        public static IllegalMigrantEntityDocument Map(this Client.IllegalMigrantEntity source)
        {
            if (source == null)
            {
                return null;
            }

            var illegalMigantEntityDocument = new IllegalMigrantEntityDocument
            {
                AccomodationPlaceID = source.AccomodationPlaceID,
                PersonalIdentityCode = source.PersonalIdentityCode,
                FirstName = source.FirstName,
                MiddleName = source.MiddleName,
                LastName = source.LastName,
                Gender = source.Gender,
                DateOfBirth = source.DateOfBirth,
                OriginCountry = source.OriginCountry,
                Religion = source.Religion
            };

            return illegalMigantEntityDocument;
        }
    }
}
