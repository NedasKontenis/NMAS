using System;
using System.ComponentModel.DataAnnotations;

namespace NMAS.WebApi.Contracts.AccommodationPlaceEntity
{
    public class FilterAccommodationPlaceEntityPagination
    {
        [Range(1, 1000)]
        public int Limit { get; set; }

        [Range(0, int.MaxValue)]
        public int Offset { get; set; }

        public FilterAccommodationPlaceEntityOrderBy OrderBy { get; set; }

        public FilterAccommodationPlaceEntityOrderDirection OrderDirection { get; set; }
    }
}
