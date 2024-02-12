using System.Collections.Generic;

namespace NMAS.WebApi.Repositories.Models.IllegalMigrantEntity
{
    public class FilterIllegalMigrantEntity
    {
        public int Limit { get; set; }

        public int Offset { get; set; }

        public FilterIllegalMigrantEntityOrderBy? OrderBy { get; set; }

        public FilterIllegalMigrantEntityOrderDirection OrderDirection { get; set; }

        public List<string> Ids { get; set; }

        public List<string> AccommodationPlaceIds { get; set; }

        public List<string> PersonalIdentityCodes { get; set; }

        public List<string> FirstNames { get; set; }

        public List<string> LastNames { get; set; }

        public List<string> Genders { get; set; }

        public FilterDateOfBirth DateOfBirth { get; set; }

        public List<string> OriginCountries { get; set; }

        public List<string> Religions { get; set; }
    }
}
