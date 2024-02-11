using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using NMAS.WebApi.Contracts.AccommodationPlaceEntity;
using NMAS.WebApi.Contracts.Exceptions;
using NMAS.WebApi.Contracts.IllegalMigrantEntity;
using NMAS.WebApi.Repositories.IllegalMigrantEntity;
using NMAS.WebApi.Repositories.Models.IllegalMigrantEntity;
using NMAS.WebApi.Services.AccommodationPlaceEntityService;
using NMAS.WebApi.Services.IllegalMigrantEntity;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace NMAS.WebApi.Unit.testss
{
    [Trait("Category", "Unit")]
    public class IllegalMigrantEntityServicetestss
    {
        private readonly Mock<IIllegalMigrantEntityRepository> _mockIllegalMigrantEntityRepository;
        private readonly Mock<IAccommodationPlaceEntityService> _mockAccommodationPlaceEntityService;
        private readonly IllegalMigrantEntityService _service;

        public IllegalMigrantEntityServicetestss()
        {
            _mockIllegalMigrantEntityRepository = new Mock<IIllegalMigrantEntityRepository>();
            _mockAccommodationPlaceEntityService = new Mock<IAccommodationPlaceEntityService>();
            _service = new IllegalMigrantEntityService(_mockIllegalMigrantEntityRepository.Object, _mockAccommodationPlaceEntityService.Object);
        }

        [Theory, AutoData]
        public async Task CreateAsync_ShouldReturnCreatedEntity(CreateIllegalMigrantEntity createRequest)
        {
            // Arrange
            var expectedId = 1;
            _mockIllegalMigrantEntityRepository.Setup(r => r.CreateAsync(It.IsAny<IllegalMigrantEntityDocument>()))
                .ReturnsAsync(expectedId);

            // Act
            var result = await _service.CreateAsync(createRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedId, result.Id);
            _mockIllegalMigrantEntityRepository.Verify(r => r.CreateAsync(It.IsAny<IllegalMigrantEntityDocument>()), Times.Once);
        }

        [Theory, AutoData]
        public async Task GetAsync_ShouldReturnEntity_WhenEntityExists(
            int id,
            IllegalMigrantEntityDocument expectedEntity)
        {
            // Arrange
            _mockIllegalMigrantEntityRepository.Setup(r => r.GetAsync(id)).ReturnsAsync(expectedEntity);

            // Act
            var result = await _service.GetAsync(id);

            // Assert
            Assert.NotNull(result);
            result.Should().BeEquivalentTo(expectedEntity, options => options.ComparingByMembers<IllegalMigrantEntity>());
        }

        [Theory, AutoData]
        public async Task UpdateAsync_ShouldUpdateEntity_WhenEntityExists(
            int id,
            UpdateIllegalMigrantEntity updateRequest)
        {
            // Arrange
            _mockIllegalMigrantEntityRepository.Setup(r => r.GetAsync(id)).ReturnsAsync(new IllegalMigrantEntityDocument());

            // Act
            await _service.UpdateAsync(id, updateRequest);

            // Assert
            _mockIllegalMigrantEntityRepository.Verify(r => r.UpdateAsync(id, It.IsAny<IllegalMigrantEntityDocument>()), Times.Once);
        }

        [Theory, AutoData]
        public async Task DeleteAsync_ShouldDeleteEntity_WhenEntityExists(int id)
        {
            // Arrange
            _mockIllegalMigrantEntityRepository.Setup(r => r.GetAsync(id)).ReturnsAsync(new IllegalMigrantEntityDocument());

            // Act
            await _service.DeleteAsync(id);

            // Assert
            _mockIllegalMigrantEntityRepository.Verify(r => r.DeleteAsync(id), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        public async Task AssignAsync_ShouldAssignToCompatibleAccommodation(int id)
        {
            // Arrange
            var places = new List<AccommodationPlaceEntity>
            {
                new AccommodationPlaceEntity { ID = 1, AccommodationCapacity = 1000, UsedAccommodationCapacity = 250 },
                new AccommodationPlaceEntity { ID = 2, AccommodationCapacity = 500, UsedAccommodationCapacity = 500 }, // Fully occupied
                // Add more places as needed
            };

            var migrantToAssign = new IllegalMigrantEntityDocument
            {
                AccommodationPlaceID = null,
            };

            _mockIllegalMigrantEntityRepository.Setup(r => r.GetAsync(id)).ReturnsAsync(migrantToAssign);
            _mockIllegalMigrantEntityRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<IllegalMigrantEntityDocument>());
            _mockAccommodationPlaceEntityService.Setup(s => s.GetAllAccommodationPlacesAsync()).ReturnsAsync(places);

            // Act
            await _service.AssignAsync(id);

            // Assert
            _mockIllegalMigrantEntityRepository.Verify(r => r.UpdateAsync(id, It.Is<IllegalMigrantEntityDocument>(m => m.AccommodationPlaceID.HasValue && m.AccommodationPlaceID.Value == 1)), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        public async Task AssignAsync_ShouldThrowWhenNoAccommodationDueToCapacityConstraints(int id)
        {
            // Arrange
            var places = new List<AccommodationPlaceEntity>
            {
                new AccommodationPlaceEntity { ID = 1, AccommodationCapacity = 1000, UsedAccommodationCapacity = 1000 }, // Fully occupied
                new AccommodationPlaceEntity { ID = 2, AccommodationCapacity = 500, UsedAccommodationCapacity = 500 }, // Fully occupied
            };

            var migrantToAssign = new IllegalMigrantEntityDocument
            {
                AccommodationPlaceID = null,
            };

            _mockIllegalMigrantEntityRepository.Setup(r => r.GetAsync(id)).ReturnsAsync(migrantToAssign);
            _mockAccommodationPlaceEntityService.Setup(s => s.GetAllAccommodationPlacesAsync()).ReturnsAsync(places);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(() => _service.AssignAsync(id));
            Assert.Equal("No suitable accommodation place found based on capacity and religious compatibility", exception.Message);

            _mockIllegalMigrantEntityRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<IllegalMigrantEntityDocument>()), Times.Never);
        }
    }
}
