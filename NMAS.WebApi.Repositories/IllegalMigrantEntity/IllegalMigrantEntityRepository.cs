using Dapper;
using NMAS.WebApi.Repositories.Bases;
using NMAS.WebApi.Repositories.Models.IllegalMigrantEntity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace NMAS.WebApi.Repositories.IllegalMigrantEntity
{
    public class IllegalMigrantEntityRepository : RepositoryBase, IIllegalMigrantEntityRepository
    {
        public IllegalMigrantEntityRepository(RepositoryConfiguration repositoryConfiguration)
            : base(repositoryConfiguration)
        { }

        private const string IllegalMigrantTable = "[dbo].[IllegalMigrant]";
        private const string ListAsyncQueryTemplate = @"SELECT * FROM [dbo].[IllegalMigrant] /**where**/ /**orderby**/ OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY";

        public async Task<int> CreateAsync(IllegalMigrantEntityDocument document)
        {
            using (var db = await OpenAsync())
            {
                try
                {
                    return await InsertAsync(db, IllegalMigrantTable, document);
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.ToString());
                }
            }
        }

        public async Task<IllegalMigrantEntityDocument> GetAsync(int id)
        {
            using (var db = await OpenAsync())
            {
                return await GetAsync<IllegalMigrantEntityDocument>(db, IllegalMigrantTable, id);
            }
        }

        public async Task UpdateAsync(int id, IllegalMigrantEntityDocument document)
        {
            using (var db = await OpenAsync())
            {
                await UpdateAsync(db, IllegalMigrantTable, id, document);
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var db = await OpenAsync())
            {
                await DeleteAsync(db, IllegalMigrantTable, id);
            }
        }

        public async Task<IEnumerable<IllegalMigrantEntityDocument>> GetAllAsync()
        {
            using (var db = await OpenAsync())
            {
                return await GetAllAsync<IllegalMigrantEntityDocument>(db, IllegalMigrantTable);
            }
        }

        public async Task<IEnumerable<IllegalMigrantEntityDocument>> ListAsync(FilterIllegalMigrantEntity filter)
        {
            var builder = new SqlBuilder();
            var template = builder
                .AddTemplate(ListAsyncQueryTemplate, new
                {
                    filter.Offset,
                    filter.Limit
                });

            if (filter.Ids != null && filter.Ids.Any())
            {
                var entityIds = filter.Ids.Select(value => new DbString
                {
                    Value = value,
                    IsFixedLength = true,
                    Length = 256,
                    IsAnsi = true
                });

                builder.Where("Id in @EntityIds", new { EntityIds = entityIds });
            }

            if (filter.AccommodationPlaceIds != null && filter.AccommodationPlaceIds.Any())
            {
                var accommodationPlaceIds = filter.AccommodationPlaceIds.Select(value => new DbString
                {
                    Value = value,
                    IsFixedLength = true,
                    Length = 256,
                    IsAnsi = true
                });

                builder.Where("AccommodationPlaceId in @AccommodationPlaceIds", new { AccommodationPlaceIds = accommodationPlaceIds });
            }

            if (filter.PersonalIdentityCodes != null && filter.PersonalIdentityCodes.Any())
            {
                var personalIdentityCodes = filter.PersonalIdentityCodes.Select(value => new DbString
                {
                    Value = value,
                    IsFixedLength = true,
                    Length = 256,
                    IsAnsi = true
                });

                builder.Where("PersonalIdentityCode in @PersonalIdentityCodes", new { PersonalIdentityCodes = personalIdentityCodes });
            }

            if (filter.FirstNames != null && filter.FirstNames.Any())
            {
                var firstNames = filter.FirstNames.Select(value => new DbString
                {
                    Value = value,
                    IsFixedLength = true,
                    Length = 256,
                    IsAnsi = true
                });

                builder.Where("FirstName in @FirstNames", new { FirstNames = firstNames });
            }

            if (filter.LastNames != null && filter.LastNames.Any())
            {
                var lastNames = filter.LastNames.Select(value => new DbString
                {
                    Value = value,
                    IsFixedLength = true,
                    Length = 256,
                    IsAnsi = true
                });

                builder.Where("LastName in @LastNames", new { LastNames = lastNames });
            }

            if (filter.Genders != null && filter.Genders.Any())
            {
                var genders = filter.Genders.Select(value => new DbString
                {
                    Value = value,
                    IsFixedLength = true,
                    Length = 256,
                    IsAnsi = true
                });

                builder.Where("Gender in @Genders", new { Genders = genders });
            }

            if (filter.DateOfBirth != null)
            {
                builder.Where("DateOfBirth between @From and @To", new { filter.DateOfBirth.From, filter.DateOfBirth.To });
            }

            if (filter.OriginCountries != null && filter.OriginCountries.Any())
            {
                var originCountries = filter.OriginCountries.Select(value => new DbString
                {
                    Value = value,
                    IsFixedLength = true,
                    Length = 256,
                    IsAnsi = true
                });

                builder.Where("OriginCountry in @OriginCountries", new { OriginCountries = originCountries });
            }

            if (filter.Religions != null && filter.Religions.Any())
            {
                var religions = filter.Religions.Select(value => new DbString
                {
                    Value = value,
                    IsFixedLength = true,
                    Length = 256,
                    IsAnsi = true
                });

                builder.Where("Religion in @Religions", new { Religions = religions });
            }

            if (filter.OrderBy.HasValue)
            {
                builder.OrderBy($"{filter.OrderBy} {filter.OrderDirection}");
            }

            using (var db = await OpenAsync())
            {
                var results = await db.QueryAsync<IllegalMigrantEntityDocument>(template.RawSql, template.Parameters);
                return results.ToList();
            }
        }
    }
}
