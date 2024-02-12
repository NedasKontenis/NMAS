using System.Collections.Generic;

namespace NMAS.WebApi.Repositories.Models.AccommodationPlaceEntity
{
    public class FilterAccommodationPlaceEntity
    {
        public int Limit { get; set; }

        public int Offset { get; set; }

        public FilterAccommodationPlaceEntityOrderBy? OrderBy { get; set; }

        public FilterAccommodationPlaceEntityOrderDirection OrderDirection { get; set; }

        public List<string> Ids { get; set; }

        public List<string> WorkerIds { get; set; }

        public List<string> PlaceNames { get; set; }

        public List<string> Adresses { get; set; }

        public List<string> CompanyCodes { get; set; }
    }
}