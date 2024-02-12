using System;

namespace NMAS.WebApi.Repositories.Models.IllegalMigrantEntity
{
    public class FilterDateOfBirth
    {
        public FilterDateOfBirth(DateTime from, DateTime to)
        {
            From = from;
            To = to;
        }

        public DateTime From { get; set; }

        public DateTime To { get; set; }
    }
}
