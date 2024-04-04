using AutoFixture.NUnit3;
using Dapper;
using NMAS.WebApi.Contracts.AccommodationPlaceEntity;
using NMAS.WebApi.Contracts.IllegalMigrantEntity;
using NMAS.WebApi.Contracts.WorkerEntity;
using NMAS.WebApi.Integration.tests;
using NUnit.Framework;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace NMAS.WebApi.Integration.Tests
{
    [TestFixture]
    [Category("Integration")]
    public class IllegalMigrantEntityIntegrationTests : TestBase
    {
        [Test, AutoData]
        public async Task CreateIllegalMigrantEntity_ShouldAddEntity(
            IllegalMigrantEntity migrant,
            WorkerEntity worker,
            AccommodationPlaceEntity place)
        {
            using (var transaction = TestsDbConnection.BeginTransaction())
            {
                var workerId = await InsertWorkerAsync(worker, transaction);
                var placeId = await InsertAccommodationPlaceAsync(place, workerId, transaction);
                var migrantId = await InsertIllegalMigrantAsync(migrant, placeId, transaction);

                var result = await TestsDbConnection.QuerySingleOrDefaultAsync<IllegalMigrantEntity>(
                    "SELECT * FROM IllegalMigrant WHERE Id = @Id;", new { Id = migrantId }, transaction: transaction);

                Assert.NotNull(result);
                Assert.AreEqual(migrant.FirstName, result.FirstName);
                Assert.AreEqual(migrant.LastName, result.LastName);
                Assert.AreEqual(placeId, result.AccommodationPlaceId);

                transaction.Rollback();
            }
        }

        [Test, AutoData]
        public async Task CreateIllegalMigrantEntity_WithNonExistentAccommodationPlace_ShouldFail(
            IllegalMigrantEntity migrant,
            WorkerEntity worker)
        {
            using (var transaction = TestsDbConnection.BeginTransaction())
            {
                var workerId = await InsertWorkerAsync(worker, transaction);
                // Intentionally skipping the insertion of an accommodation place to simulate a failure scenario
                // var placeId = await InsertAccommodationPlaceAsync(place, workerId, transaction);
                var invalidPlaceId = -1; // Using an obviously invalid ID for accommodation place

                // Attempting to insert an illegal migrant with a non-existent accommodation place ID
                // This operation should fail, so we do not expect to successfully retrieve an ID for the migrant
                var migrantId = 0;
                try
                {
                    migrantId = await InsertIllegalMigrantAsync(migrant, invalidPlaceId, transaction);
                }
                catch (Exception ex)
                {
                    // Expecting an exception due to foreign key constraint violation
                    Assert.IsInstanceOf<SqlException>(ex);
                }

                // Verifying that no illegal migrant entity was created
                if (migrantId != 0)
                {
                    var result = await TestsDbConnection.QuerySingleOrDefaultAsync<IllegalMigrantEntity>(
                        "SELECT * FROM IllegalMigrant WHERE Id = @Id;", new { Id = migrantId }, transaction: transaction);

                    Assert.IsNull(result); // Asserting that no result is found since the insertion should have failed
                }

                transaction.Rollback();
            }
        }

        [Test, AutoData]
        public async Task UpdateIllegalMigrantEntity_ShouldModifyEntity(
            IllegalMigrantEntity migrant,
            WorkerEntity worker,
            AccommodationPlaceEntity place,
            string updatedLastName)
        {
            using (var transaction = TestsDbConnection.BeginTransaction())
            {
                var workerId = await InsertWorkerAsync(worker, transaction);
                var placeId = await InsertAccommodationPlaceAsync(place, workerId, transaction);
                var migrantId = await InsertIllegalMigrantAsync(migrant, placeId, transaction);

                await TestsDbConnection.ExecuteAsync(
                    "UPDATE IllegalMigrant SET LastName = @UpdatedLastName WHERE Id = @Id;",
                    new { UpdatedLastName = updatedLastName, Id = migrantId }, transaction: transaction);

                var updatedMigrant = await TestsDbConnection.QuerySingleOrDefaultAsync<IllegalMigrantEntity>(
                    "SELECT * FROM IllegalMigrant WHERE Id = @Id;", new { Id = migrantId }, transaction: transaction);

                Assert.AreEqual(updatedLastName, updatedMigrant.LastName);

                transaction.Rollback();
            }
        }

        [Test, AutoData]
        public async Task DeleteIllegalMigrantEntity_ShouldRemoveEntity(
            IllegalMigrantEntity migrant,
            WorkerEntity worker,
            AccommodationPlaceEntity place)
        {
            using (var transaction = TestsDbConnection.BeginTransaction())
            {
                var workerId = await InsertWorkerAsync(worker, transaction);
                var placeId = await InsertAccommodationPlaceAsync(place, workerId, transaction);
                var migrantId = await InsertIllegalMigrantAsync(migrant, placeId, transaction);

                await TestsDbConnection.ExecuteAsync(
                    "DELETE FROM IllegalMigrant WHERE Id = @Id;",
                    new { Id = migrantId }, transaction: transaction);

                var result = await TestsDbConnection.QuerySingleOrDefaultAsync<IllegalMigrantEntity>(
                    "SELECT * FROM IllegalMigrant WHERE Id = @Id;", new { Id = migrantId }, transaction: transaction);

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
            place.WorkerId = workerId;
            return await TestsDbConnection.ExecuteScalarAsync<int>(
                @"INSERT INTO AccommodationPlace (WorkerId, PlaceName, Adress, AccommodationCapacity, UsedAccommodationCapacity, CompanyCode, ContactPhone)
          OUTPUT INSERTED.ID
          VALUES (@WorkerId, @PlaceName, @Adress, @AccommodationCapacity, @UsedAccommodationCapacity, @CompanyCode, @ContactPhone);",
                place, transaction: transaction);
        }

        private async Task<int> InsertIllegalMigrantAsync(IllegalMigrantEntity migrant, int placeId, SqlTransaction transaction)
        {
            migrant.AccommodationPlaceId = placeId;
            return await TestsDbConnection.ExecuteScalarAsync<int>(
                @"INSERT INTO IllegalMigrant (AccommodationPlaceId, PersonalIdentityCode, FirstName, MiddleName, LastName, Gender, DateOfBirth, OriginCountry, Religion)
          OUTPUT INSERTED.ID
          VALUES (@AccommodationPlaceId, @PersonalIdentityCode, @FirstName, @MiddleName, @LastName, @Gender, @DateOfBirth, @OriginCountry, @Religion);",
                migrant, transaction: transaction);
        }
    }
}
