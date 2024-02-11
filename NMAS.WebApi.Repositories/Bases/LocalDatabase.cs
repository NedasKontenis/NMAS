using DbUp;

namespace NMAS.WebApi.Repositories.Bases
{
    public class LocalDatabase
    {
        private readonly string _connectionString;

        public LocalDatabase()
        {
            _connectionString = @"Server=(LocalDB)\MSSQLLocalDB;Initial Catalog=db-nmas;Trusted_Connection=True;";
        }
        public string EnsureCreated()
        {
            EnsureDatabase.For.SqlDatabase(_connectionString);
            return _connectionString;
        }

        public void EnsureDropped()
        {
            DropDatabase.For.SqlDatabase(_connectionString);
        }
    }
}
