using Dapper;
using System.Collections.Generic;
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

        protected async Task<int> InsertAsync(IDbConnection db, string tableName, object document, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var properties = document.GetType().GetProperties();
            var columnNames = string.Join(", ", properties.Select(p => $"[{p.Name}]"));
            var valueNames = string.Join(", ", properties.Select(p => $"@{p.Name}"));

            var sql = $@"INSERT INTO {tableName} ({columnNames}) 
                 VALUES ({valueNames});
                 SELECT CAST(SCOPE_IDENTITY() as int);";

            int id = await db.QuerySingleAsync<int>(sql, document, transaction, commandTimeout, commandType);

            return id;
        }

        protected async Task<T> GetAsync<T>(IDbConnection db, string tableName, int id, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var sql = $@"SELECT * FROM {tableName} WHERE [Id] = @Id";

            var result = await db.QueryAsync<T>(sql, new
            {
                Id = id
            }, transaction, commandTimeout, commandType);
            return result.FirstOrDefault();
        }

        protected async Task UpdateAsync<T>(IDbConnection db, string tableName, int id, T document, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var properties = typeof(T).GetProperties().Where(prop => prop.Name != "ID");
            var setClauses = new List<string>();
            foreach (var prop in properties)
            {
                setClauses.Add($"{prop.Name} = @{prop.Name}");
            }
            var setClause = string.Join(", ", setClauses);
            var sql = $@"UPDATE {tableName} SET {setClause} WHERE ID = @Id";

            var parameters = new DynamicParameters();
            foreach (var prop in properties)
            {
                parameters.Add($"@{prop.Name}", prop.GetValue(document));
            }
            parameters.Add("@Id", id);

            await db.ExecuteAsync(sql, parameters, transaction, commandTimeout, commandType);
        }

        protected async Task DeleteAsync(IDbConnection db, string tableName, int id, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var sql = $@"DELETE {tableName} WHERE ID = @Id";

            await db.ExecuteAsync(sql, new
            {
                Id = id
            }, transaction, commandTimeout, commandType);
        }

        protected async Task<IEnumerable<T>> GetAllAsync<T>(IDbConnection db, string tableName, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var sql = $@"SELECT * FROM {tableName}";

            var result = await db.QueryAsync<T>(sql, transaction: transaction, commandTimeout: commandTimeout, commandType: commandType);
            return result;
        }
    }
}
