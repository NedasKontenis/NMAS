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
    }
}
