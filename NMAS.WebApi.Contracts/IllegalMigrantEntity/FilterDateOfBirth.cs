using System;
using System.ComponentModel.DataAnnotations;

namespace NMAS.WebApi.Contracts.IllegalMigrantEntity
{
    public class FilterDateOfBirth
    {
        /// <summary>
        /// Date time from interval to filter
        /// </summary>
        [Range(typeof(DateTime), "1/1/1753", "12/31/9999")]
        public DateTime From { get; set; }

        /// <summary>
        /// Date time to interval to filter
        /// </summary>
        [Range(typeof(DateTime), "1/1/1753", "12/31/9999")]
        public DateTime To { get; set; }
    }
}
