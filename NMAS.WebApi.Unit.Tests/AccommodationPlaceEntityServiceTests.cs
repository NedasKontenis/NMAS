using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using NMAS.WebApi.Contracts.AccommodationPlaceEntity;
using NMAS.WebApi.Contracts.AccomodationPlaceEntity;
using NMAS.WebApi.Repositories.AccommodationPlaceEntity;
using NMAS.WebApi.Repositories.Models.AccommodationPlaceEntity;
using NMAS.WebApi.Services.AccommodationPlaceEntityService;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace NMAS.WebApi.Unit.testss
{
    [Trait("Category", "Unit")]
    public class AccommodationPlaceEntityServicetestss
    {
        private readonly Mock<IAccommodationPlaceEntityRepository> _mockRepository;
        private readonly AccommodationPlaceEntityService _service;

        public AccommodationPlaceEntityServicetestss()
        {
            _mockRepository = new Mock<IAccommodationPlaceEntityRepository>();
            _service = new AccommodationPlaceEntityService(_mockRepository.Object);
        }

        [Theory, AutoData]
        public async Task CreateAsync_ShouldReturnCreatedEntity(
            CreateAccommodationPlaceEntity createRequest,
            int expectedId)
        {
            // Arrange
            _mockRepository.Setup(repo => repo.CreateAsync(It.IsAny<AccommodationPlaceEntityDocument>()))
                .ReturnsAsync(expectedId);

            // Act
            var result = await _service.CreateAsync(createRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedId, result.Id);
            _mockRepository.Verify(repo => repo.CreateAsync(It.IsAny<AccommodationPlaceEntityDocument>()), Times.Once);
        }

        [Theory, AutoData]
        public async Task GetAsync_ShouldReturnEntity_WhenEntityExists(
        int testsId,
        AccommodationPlaceEntityDocument expectedDocument)
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAsync(testsId))
                .ReturnsAsync(expectedDocument);

            // Act
            var result = await _service.GetAsync(testsId);

            // Assert
            result.Should().BeEquivalentTo(expectedDocument, options => options.ComparingByMembers<AccommodationPlaceEntityDocument>());
        }

        [Theory, AutoData]
        public async Task UpdateAsync_ShouldInvokeRepository_WhenEntityExists(
        int testsId,
        UpdateAccommodationPlaceEntity updateRequest,
        AccommodationPlaceEntityDocument existingDocument)
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAsync(testsId))
                .ReturnsAsync(existingDocument);

            // Act
            await _service.UpdateAsync(testsId, updateRequest);

            // Assert
            _mockRepository.Verify(repo => repo.UpdateAsync(testsId, It.IsAny<AccommodationPlaceEntityDocument>()), Times.Once);
        }

        [Theory, AutoData]
        public async Task DeleteAsync_ShouldInvokeRepository_WhenEntityExists(
            int testsId,
            AccommodationPlaceEntityDocument existingDocument)
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAsync(testsId))
                .ReturnsAsync(existingDocument);

            // Act
            await _service.DeleteAsync(testsId);

            // Assert
            _mockRepository.Verify(repo => repo.DeleteAsync(testsId), Times.Once);
        }

        [Theory, AutoData]
        public async Task GetAllAccommodationPlacesAsync_ShouldReturnAllPlaces(
            List<AccommodationPlaceEntityDocument> expectedPlaces)
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAllAccommodationPlacesAsync())
                .ReturnsAsync(expectedPlaces);

            // Act
            var result = await _service.GetAllAccommodationPlacesAsync();

            // Assert
            result.Should().BeEquivalentTo(expectedPlaces, options => options.ComparingByMembers<AccommodationPlaceEntityDocument>());
        }

        [Theory, AutoData]
        public async Task IncrementUsedAccommodationCapacity_ShouldIncrementCapacity(
            int placeId,
            AccommodationPlaceEntityDocument placeBeforeIncrement)
        {
            // Arrange
            placeBeforeIncrement.UsedAccommodationCapacity = 5;
            var expectedCapacityAfterIncrement = placeBeforeIncrement.UsedAccommodationCapacity + 1;
            _mockRepository.Setup(repo => repo.GetAsync(placeId))
                .ReturnsAsync(placeBeforeIncrement);
            _mockRepository.Setup(repo => repo.UpdateAsync(placeId, It.IsAny<AccommodationPlaceEntityDocument>()))
                .Callback<int, AccommodationPlaceEntityDocument>((_, updatedPlace) =>
                {
                    updatedPlace.UsedAccommodationCapacity.Should().Be(expectedCapacityAfterIncrement);
                })
                .Returns(Task.CompletedTask);

            // Act
            await _service.IncrementUsedAccommodationCapacity(placeId);

            // Assert
            _mockRepository.Verify(repo => repo.UpdateAsync(placeId, It.IsAny<AccommodationPlaceEntityDocument>()), Times.Once);
        }

        [Theory, AutoData]
        public async Task DecrementUsedAccommodationCapacity_ShouldDecrementCapacity(
            int placeId,
            AccommodationPlaceEntityDocument placeBeforeDecrement)
        {
            // Arrange
            placeBeforeDecrement.UsedAccommodationCapacity = 5;
            var expectedCapacityAfterDecrement = placeBeforeDecrement.UsedAccommodationCapacity - 1;
            _mockRepository.Setup(repo => repo.GetAsync(placeId))
                .ReturnsAsync(placeBeforeDecrement);
            _mockRepository.Setup(repo => repo.UpdateAsync(placeId, It.IsAny<AccommodationPlaceEntityDocument>()))
                .Callback<int, AccommodationPlaceEntityDocument>((_, updatedPlace) =>
                {
                    updatedPlace.UsedAccommodationCapacity.Should().Be(expectedCapacityAfterDecrement);
                })
                .Returns(Task.CompletedTask);

            // Act
            await _service.DecrementUsedAccommodationCapacity(placeId);

            // Assert
            _mockRepository.Verify(repo => repo.UpdateAsync(placeId, It.IsAny<AccommodationPlaceEntityDocument>()), Times.Once);
        }
    }
}