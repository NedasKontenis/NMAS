using System.Collections.Generic;

namespace NMAS.WebApi.Contracts.AccommodationPlaceEntity
{
    public class FilterAccommodationPlaceEntity
    {
        /// <summary>
        /// The accommodation place ids to filter
        /// </summary>
        public List<string> Ids { get; set; }

        /// <summary>
        /// The Worker IDs to filter
        /// </summary>
        public List<string> WorkerIds { get; set; }

        /// <summary>
        /// The place names to filter
        /// </summary>
        public List<string> PlaceNames { get; set; }

        /// <summary>
        /// The addresses to filter
        /// </summary>
        public List<string> Adresses { get; set; }

        /// <summary>
        /// The company codes to filter
        /// </summary>
        public List<string> CompanyCodes { get; set; }
    }
}
