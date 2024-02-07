using Dapper;
using NMAS.WebApi.Repositories.Models.IllegalMigrantEntity;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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

        protected async Task<int> InsertAsync<T>(IDbConnection db, string tableName, T document, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        where T : IllegalMigrantEntityDocument
        {
            var sql = $@"INSERT INTO {tableName}([AccommodationPlaceID],[PersonalIdentityCode],[FirstName],[MiddleName],[LastName],[Gender],[DateOfBirth],[OriginCountry],[Religion]) 
                 VALUES (@AccommodationPlaceID, @PersonalIdentityCode, @FirstName, @MiddleName, @LastName, @Gender, @DateOfBirth, @OriginCountry, @Religion);
                 SELECT CAST(SCOPE_IDENTITY() as int);";

            int id = await db.QuerySingleAsync<int>(sql, new
            {
                document.AccommodationPlaceID,
                document.PersonalIdentityCode,
                document.FirstName,
                document.MiddleName,
                document.LastName,
                document.Gender,
                document.DateOfBirth,
                document.OriginCountry,
                document.Religion
            }, transaction, commandTimeout, commandType);

            return id;
        }

        protected async Task<IllegalMigrantEntityDocument> GetAsync<T>(IDbConnection db, string tableName, int id, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) where T : class
        {
            var sql = $@"SELECT * FROM {tableName} WHERE [Id] = @Id";

            var result = await db.QueryAsync<T>(sql, new
            {
                Id = id
            }, transaction, commandTimeout, commandType);
            return result.FirstOrDefault() as IllegalMigrantEntityDocument;
        }

        protected async Task UpdateAsync(IDbConnection db, string tableName, int id, IllegalMigrantEntityDocument document, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var sql = $@"UPDATE {tableName} 
                         SET AccommodationPlaceID = @AccommodationPlaceID,
                             PersonalIdentityCode = @PersonalIdentityCode,
                             FirstName = @FirstName,
                             MiddleName = @MiddleName,
                             LastName = @LastName,
                             Gender = @Gender,
                             DateOfBirth = @DateOfBirth,
                             OriginCountry = @OriginCountry,
                             Religion = @Religion
                         WHERE ID = @Id";

            await db.ExecuteAsync(sql, new
            {
                document.AccommodationPlaceID,
                document.PersonalIdentityCode,
                document.FirstName,
                document.MiddleName,
                document.LastName,
                document.Gender,
                document.DateOfBirth,
                document.OriginCountry,
                document.Religion,
                Id = id
            }, transaction, commandTimeout, commandType);
        }

        protected async Task DeleteAsync(IDbConnection db, string tableName, int id, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var sql = $@"DELETE {tableName} WHERE ID = @Id";

            await db.ExecuteAsync(sql, new
            {
                Id = id
            }, transaction, commandTimeout, commandType);
        }
    }
}
