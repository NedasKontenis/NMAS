using Dapper;
using NMAS.WebApi.Repositories.Bases;
using NMAS.WebApi.Repositories.Models.AccommodationPlaceEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace NMAS.WebApi.Repositories.AccommodationPlaceEntity
{
    public class AccommodationPlaceEntityRepository : RepositoryBase, IAccommodationPlaceEntityRepository
    {
        public AccommodationPlaceEntityRepository(RepositoryConfiguration repositoryConfiguration)
            : base(repositoryConfiguration)
        { }

        private const string AccommodationPlaceTable = "[dbo].[AccommodationPlace]";
        private const string ListAsyncQueryTemplate = @"SELECT * FROM [dbo].[AccommodationPlace] /**where**/ /**orderby**/ OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY";

        public async Task<int> CreateAsync(AccommodationPlaceEntityDocument document)
        {
            using (var db = await OpenAsync())
            {
                try
                {
                    return await InsertAsync(db, AccommodationPlaceTable, document);
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.ToString());
                }
            }
        }

        public async Task<AccommodationPlaceEntityDocument> GetAsync(int id)
        {
            using (var db = await OpenAsync())
            {
                return await GetAsync<AccommodationPlaceEntityDocument>(db, AccommodationPlaceTable, id);
            }
        }

        public async Task UpdateAsync(int id, AccommodationPlaceEntityDocument document)
        {
            using (var db = await OpenAsync())
            {
                await UpdateAsync(db, AccommodationPlaceTable, id, document);
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var db = await OpenAsync())
            {
                await DeleteAsync(db, AccommodationPlaceTable, id);
            }
        }

        public async Task<IEnumerable<AccommodationPlaceEntityDocument>> GetAllAccommodationPlacesAsync()
        {
            using (var db = await OpenAsync())
            {
                return await GetAllAsync<AccommodationPlaceEntityDocument>(db, AccommodationPlaceTable);
            }
        }

        public async Task<IEnumerable<AccommodationPlaceEntityDocument>> ListAsync(FilterAccommodationPlaceEntity filter)
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

            if (filter.WorkerIds != null && filter.WorkerIds.Any())
            {
                var workerIds = filter.WorkerIds.Select(value => new DbString
                {
                    Value = value,
                    IsFixedLength = true,
                    Length = 256,
                    IsAnsi = true
                });

                builder.Where("WorkerId in @WorkerIds", new { WorkerIds = workerIds });
            }

            if (filter.PlaceNames != null && filter.PlaceNames.Any())
            {
                var placeNames = filter.PlaceNames.Select(value => new DbString
                {
                    Value = value,
                    IsFixedLength = true,
                    Length = 256,
                    IsAnsi = true
                });

                builder.Where("PlaceName in @PlaceNames", new { PlaceNames = placeNames });
            }

            if (filter.Adresses != null && filter.Adresses.Any())
            {
                var adresses = filter.Adresses.Select(value => new DbString
                {
                    Value = value,
                    IsFixedLength = true,
                    Length = 256,
                    IsAnsi = true
                });

                builder.Where("Adress in @Adresses", new { Adresses = adresses });
            }

            if (filter.CompanyCodes != null && filter.CompanyCodes.Any())
            {
                var companyCodes = filter.CompanyCodes.Select(value => new DbString
                {
                    Value = value,
                    IsFixedLength = true,
                    Length = 256,
                    IsAnsi = true
                });

                builder.Where("CompanyCode in @CompanyCodes", new { CompanyCodes = companyCodes });
            }

            if (filter.OrderBy.HasValue)
            {
                builder.OrderBy($"{filter.OrderBy} {filter.OrderDirection}");
            }

            using (var db = await OpenAsync())
            {
                var results = await db.QueryAsync<AccommodationPlaceEntityDocument>(template.RawSql, template.Parameters);
                return results.ToList();
            }
        }
    }
}
