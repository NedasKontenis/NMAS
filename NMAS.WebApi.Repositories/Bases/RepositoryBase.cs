using Dapper;
using NMAS.WebApi.Repositories.Models.IllegalMigrantEntity;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace NMAS.WebApi.Repositories.Bases
{
    public abstract class RepositoryBase
    {
        private readonly RepositoryConfiguration _repositoryConfiguration;

        protected RepositoryBase(RepositoryConfiguration repositoryConfiguration)
        {
            _repositoryConfiguration = repositoryConfiguration;
        }

        protected virtual async Task<SqlConnection> OpenAsync()
        {
            var db = new SqlConnection(_repositoryConfiguration.DefaultConnectionString);
            await db.OpenAsync();
            return db;
        }

        protected Task<int> InsertAsync<T>(IDbConnection db, string tableName, T document, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        where T : IllegalMigrantEntityDocument
        {
            var sql = $@"INSERT INTO {tableName}([AccomodationPlaceID],[PersonalIdentityCode],[FirstName],[MiddleName],[LastName],[Gender],[DateOfBirth],[OriginCountry],[Religion]) 
                       VALUES (@AccomodationPlaceID, @PersonalIdentityCode, @FirstName, @MiddleName, @LastName, @Gender, @DateOfBirth, @OriginCountry, @Religion)";

            return db.ExecuteAsync(sql, new
            {
                document.AccomodationPlaceID,
                document.PersonalIdentityCode,
                document.FirstName,
                document.MiddleName,
                document.LastName,
                document.Gender,
                document.DateOfBirth,
                document.OriginCountry,
                document.Religion
            }, transaction, commandTimeout, commandType);
        }

    }
}
