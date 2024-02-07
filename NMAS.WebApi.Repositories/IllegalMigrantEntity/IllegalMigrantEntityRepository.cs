using NMAS.WebApi.Repositories.Bases;
using NMAS.WebApi.Repositories.Models.IllegalMigrantEntity;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace NMAS.WebApi.Repositories.IllegalMigrantEntity
{
    public class IllegalMigrantEntityRepository : RepositoryBase, IIllegalMigrantEntityRepository
    {
        public IllegalMigrantEntityRepository(RepositoryConfiguration repositoryConfiguration)
            : base(repositoryConfiguration)
        { }

        private const string IllegalMigrantTable = "[dbo].[IllegalMigrant]";

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
    }
}
