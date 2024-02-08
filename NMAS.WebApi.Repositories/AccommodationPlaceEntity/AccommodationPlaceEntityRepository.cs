using NMAS.WebApi.Repositories.Bases;
using NMAS.WebApi.Repositories.Models.AccommodationPlaceEntity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace NMAS.WebApi.Repositories.AccommodationPlaceEntity
{
    public class AccommodationPlaceEntityRepository : RepositoryBase, IAccommodationPlaceEntityRepository
    {
        public AccommodationPlaceEntityRepository(RepositoryConfiguration repositoryConfiguration)
            : base(repositoryConfiguration)
        { }

        private const string AccommodationPlaceTable = "[dbo].[AccommodationPlace]";

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
    }
}
