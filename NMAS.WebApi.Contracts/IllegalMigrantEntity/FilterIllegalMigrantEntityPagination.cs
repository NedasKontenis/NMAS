using System;
using System.ComponentModel.DataAnnotations;

namespace NMAS.WebApi.Contracts.IllegalMigrantEntity
{
    public class FilterIllegalMigrantEntityPagination
    {
        [Range(1, 1000)]
        public int Limit { get; set; }

        [Range(0, int.MaxValue)]
        public int Offset { get; set; }

        public FilterIllegalMigrantEntityOrderBy OrderBy { get; set; }

        public FilterIllegalMigrantEntityOrderDirection OrderDirection { get; set; }
    }
}
