using DbUp;

namespace NMAS.WebApi.Repositories.Bases
{
    public class LocalDatabase
    {
        private readonly string _connectionString;

        public LocalDatabase()
        {
            _connectionString = @"Server=LAPTOP-BDJBM3A2;Initial Catalog=db-nmas;Integrated Security=True";
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
