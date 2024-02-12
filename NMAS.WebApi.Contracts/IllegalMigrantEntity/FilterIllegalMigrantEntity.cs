using System.Collections.Generic;

namespace NMAS.WebApi.Contracts.IllegalMigrantEntity
{
    public class FilterIllegalMigrantEntity
    {
        /// <summary>
        /// The illegal migrants ids to filter
        /// </summary>
        public List<string> Ids { get; set; }

        /// <summary>
        /// The accommodation place ids to filter
        /// </summary>
        public List<string> AccommodationPlaceIds { get; set; }

        /// <summary>
        /// The personal identity codes of illegal migrants to filter
        /// </summary>
        public List<string> PersonalIdentityCodes { get; set; }

        /// <summary>
        /// The first names of illegal migrants to filter
        /// </summary>
        public List<string> FirstNames { get; set; }

        /// <summary>
        /// The last names of illegal migrants to filter
        /// </summary>
        public List<string> LastNames { get; set; }

        /// <summary>
        /// The genders of illegal migrants to filter
        /// </summary>
        public List<string> Genders { get; set; }

        /// <summary>
        /// The dateOfBirth range to filter
        /// </summary>
        public FilterDateOfBirth DateOfBirth { get; set; }

        /// <summary>
        /// The origin countries of illegal migrants to filter
        /// </summary>
        public List<string> OriginCountries { get; set; }

        /// <summary>
        /// The religions of illegal migrants to filter
        /// </summary>
        public List<string> Religions { get; set; }
    }
}
