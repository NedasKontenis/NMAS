using DbUp;
using NUnit.Framework;
using System;
using System.Data.SqlClient;
using System.Reflection;
using System.Threading.Tasks;

namespace NMAS.WebApi.Integration.testss
{
    [SetUpFixture]
    public abstract class TestBase
    {
        protected static readonly string TestsConnectionString = @"Server=(LocalDB)\MSSQLLocalDB;Initial Catalog=db-nmas-tests;Trusted_Connection=True;";
        protected static SqlConnection TestsDbConnection;

        [OneTimeSetUp]
        public async Task GlobalSetup()
        {
            DropDatabaseIfExists();
            EnsureDatabase.For.SqlDatabase(TestsConnectionString);

            var upgrader = DeployChanges.To
                .SqlDatabase(TestsConnectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToConsole()
                .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                throw new InvalidOperationException("Database setup failed.", result.Error);
            }

            TestsDbConnection = new SqlConnection(TestsConnectionString);
            await TestsDbConnection.OpenAsync();
        }

        [OneTimeTearDown]
        public async Task GlobalTeardown()
        {
            await TestsDbConnection.CloseAsync();
            DropDatabase.For.SqlDatabase(TestsConnectionString);
        }

        private static void DropDatabaseIfExists()
        {
            var masterConnectionString = new SqlConnectionStringBuilder(TestsConnectionString) { InitialCatalog = "master" }.ConnectionString;
            using (var connection = new SqlConnection(masterConnectionString))
            {
                connection.Open();
                var cmdText = $"IF EXISTS(SELECT * FROM sys.databases WHERE name = 'db-nmas-tests') BEGIN DROP DATABASE [db-nmas-tests]; END";
                using (var command = new SqlCommand(cmdText, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
