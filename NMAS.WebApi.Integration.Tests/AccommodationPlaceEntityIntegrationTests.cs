using AutoFixture.NUnit3;
using Dapper;
using NMAS.WebApi.Contracts.AccommodationPlaceEntity;
using NMAS.WebApi.Contracts.WorkerEntity;
using NUnit.Framework;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace NMAS.WebApi.Integration.testss
{
    [TestFixture]
    [Category("Integration")]
    public class AccommodationPlaceEntityIntegrationtests : TestBase
    {
        [Test, AutoData]
        public async Task CreateAccommodationPlaceEntity_ShouldAddEntity(
            AccommodationPlaceEntity place,
            WorkerEntity worker)
        {
            using (var transaction = TestsDbConnection.BeginTransaction())
            {
                var workerId = await InsertWorkerAsync(worker, transaction);
                var placeId = await InsertAccommodationPlaceAsync(place, workerId, transaction);

                var result = await TestsDbConnection.QuerySingleOrDefaultAsync<AccommodationPlaceEntity>(
                    "SELECT * FROM AccommodationPlace WHERE ID = @Id;", new { Id = placeId }, transaction: transaction);

                Assert.NotNull(result);
                Assert.AreEqual(place.PlaceName, result.PlaceName);
                Assert.AreEqual(place.Adress, result.Adress);
                Assert.AreEqual(place.AccommodationCapacity, result.AccommodationCapacity);
                Assert.AreEqual(place.UsedAccommodationCapacity, result.UsedAccommodationCapacity);

                transaction.Rollback();
            }
        }

        [Test, AutoData]
        public async Task UpdateAccommodationPlaceEntity_ShouldModifyEntity(
            AccommodationPlaceEntity place,
            WorkerEntity worker,
            string placeName)
        {
            using (var transaction = TestsDbConnection.BeginTransaction())
            {
                var workerId = await InsertWorkerAsync(worker, transaction);
                var placeId = await InsertAccommodationPlaceAsync(place, workerId, transaction);

                await TestsDbConnection.ExecuteAsync(
                    "UPDATE AccommodationPlace SET PlaceName = @PlaceName WHERE ID = @Id;",
                    new { PlaceName = placeName, Id = placeId }, transaction: transaction);

                var updatedEntity = await TestsDbConnection.QuerySingleOrDefaultAsync<AccommodationPlaceEntity>(
                    "SELECT * FROM AccommodationPlace WHERE ID = @Id;", new { Id = placeId }, transaction: transaction);

                Assert.AreEqual(placeName, updatedEntity.PlaceName);

                transaction.Rollback();
            }
        }

        [Test, AutoData]
        public async Task DeleteAccommodationPlaceEntity_ShouldRemoveEntity(
            AccommodationPlaceEntity place,
            WorkerEntity worker)
        {
            using (var transaction = TestsDbConnection.BeginTransaction())
            {
                var workerId = await InsertWorkerAsync(worker, transaction);
                var placeId = await InsertAccommodationPlaceAsync(place, workerId, transaction);

                await TestsDbConnection.ExecuteAsync(
                    "DELETE FROM AccommodationPlace WHERE ID = @Id;",
                    new { Id = placeId }, transaction: transaction);

                var result = await TestsDbConnection.QuerySingleOrDefaultAsync<AccommodationPlaceEntity>(
                    "SELECT * FROM AccommodationPlace WHERE ID = @Id;", new { Id = placeId }, transaction: transaction);

                Assert.Null(result);

                transaction.Rollback();
            }
        }

        private async Task<int> InsertWorkerAsync(WorkerEntity worker, SqlTransaction transaction)
        {
            return await TestsDbConnection.ExecuteScalarAsync<int>(
                @"INSERT INTO Worker (FirstName, LastName, PersonalIdentityCode, ContactEmail, ContactPhone)
          VALUES (@FirstName, @LastName, @PersonalIdentityCode, @ContactEmail, @ContactPhone);
          SELECT CAST(SCOPE_IDENTITY() as int);",
                worker, transaction: transaction);
        }

        private async Task<int> InsertAccommodationPlaceAsync(AccommodationPlaceEntity place, int workerId, SqlTransaction transaction)
        {
            place.WorkerID = workerId;
            return await TestsDbConnection.ExecuteScalarAsync<int>(
                @"INSERT INTO AccommodationPlace (WorkerID, PlaceName, Adress, AccommodationCapacity, UsedAccommodationCapacity, CompanyCode, ContactPhone)
          OUTPUT INSERTED.ID
          VALUES (@WorkerID, @PlaceName, @Adress, @AccommodationCapacity, @UsedAccommodationCapacity, @CompanyCode, @ContactPhone);",
                place, transaction: transaction);
        }
    }
}