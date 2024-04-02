using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using NMAS.WebApi.Contracts.AccommodationPlaceEntity;
using NMAS.WebApi.Contracts.AccomodationPlaceEntity;
using NMAS.WebApi.Contracts.Exceptions;
using NMAS.WebApi.Repositories.AccommodationPlaceEntity;
using NMAS.WebApi.Repositories.Models.AccommodationPlaceEntity;
using NMAS.WebApi.Services.AccommodationPlaceEntityService;
using NMAS.WebApi.Services.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NMAS.WebApi.Unit.tests
{
    [Trait("Category", "Unit")]
    public class AccommodationPlaceEntityServiceTests
    {
        private readonly Mock<IAccommodationPlaceEntityRepository> _mockRepository;
        private readonly AccommodationPlaceEntityService _service;

        public AccommodationPlaceEntityServiceTests()
        {
            _mockRepository = new Mock<IAccommodationPlaceEntityRepository>();
            _service = new AccommodationPlaceEntityService(_mockRepository.Object);
        }

        [Theory, AutoData]
        public async Task CreateAsync_ShouldReturnCreatedEntity(
            int resourceId,
            CreateAccommodationPlaceEntity createAccommodationPlaceEntity)
        {
            // Arrange
            _mockRepository.Setup(repo => repo.CreateAsync(It.IsAny<AccommodationPlaceEntityDocument>()))
                .ReturnsAsync(resourceId);

            // Act
            var result = await _service.CreateAsync(createAccommodationPlaceEntity);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(resourceId, result.Id);
            _mockRepository.Verify(repo => repo.CreateAsync(It.IsAny<AccommodationPlaceEntityDocument>()), Times.Once);
        }

        [Theory, AutoData]
        public async Task GetAsync_ShouldReturnEntity_WhenEntityExists(
            int resourceId,
            AccommodationPlaceEntityDocument accommodationPlaceEntityDocument)
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAsync(resourceId))
                .ReturnsAsync(accommodationPlaceEntityDocument);

            // Act
            var result = await _service.GetAsync(resourceId);

            // Assert
            result.Should().BeEquivalentTo(accommodationPlaceEntityDocument, options => options.ComparingByMembers<AccommodationPlaceEntityDocument>());
        }

        [Theory, AutoData]
        public async Task GetAsync_ShouldThrowResourceNotFoundException_WhenEntityDoesNotExist(
            int resourceId)
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAsync(resourceId))
                .ReturnsAsync((AccommodationPlaceEntityDocument)null);

            // Act & Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(() => _service.GetAsync(resourceId));
        }

        [Theory, AutoData]
        public async Task UpdateAsync_ShouldInvokeRepository_WhenEntityExists(
            int resourceId,
            UpdateAccommodationPlaceEntity updateAccommodationPlaceEntity,
            AccommodationPlaceEntityDocument accommodationPlaceEntityDocument)
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAsync(resourceId))
                .ReturnsAsync(accommodationPlaceEntityDocument);

            // Act
            await _service.UpdateAsync(resourceId, updateAccommodationPlaceEntity);

            // Assert
            _mockRepository.Verify(repo => repo.UpdateAsync(resourceId, It.IsAny<AccommodationPlaceEntityDocument>()), Times.Once);
        }

        [Theory, AutoData]
        public async Task UpdateAsync_ShouldThrowResourceNotFoundException_WhenEntityDoesNotExist(
        int resourceId,
        UpdateAccommodationPlaceEntity updateAccommodationPlaceEntity)
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAsync(resourceId))
                .ReturnsAsync((AccommodationPlaceEntityDocument)null);

            // Act & Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(() => _service.UpdateAsync(resourceId, updateAccommodationPlaceEntity));

            _mockRepository.Verify(repo => repo.GetAsync(resourceId), Times.Once);
            _mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<int>(), It.IsAny<AccommodationPlaceEntityDocument>()), Times.Never);
        }

        [Theory, AutoData]
        public async Task DeleteAsync_ShouldInvokeRepository_WhenEntityExists(
            int resourceId,
            AccommodationPlaceEntityDocument accommodationPlaceEntityDocument)
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAsync(resourceId))
                .ReturnsAsync(accommodationPlaceEntityDocument);

            // Act
            await _service.DeleteAsync(resourceId);

            // Assert
            _mockRepository.Verify(repo => repo.DeleteAsync(resourceId), Times.Once);
        }

        [Theory, AutoData]
        public async Task GetAllAccommodationPlacesAsync_ShouldReturnAllPlaces(
            List<AccommodationPlaceEntityDocument> accommodationPlaceEntityDocumentList)
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAllAccommodationPlacesAsync())
                .ReturnsAsync(accommodationPlaceEntityDocumentList);

            // Act
            var result = await _service.GetAllAccommodationPlacesAsync();

            // Assert
            result.Should().BeEquivalentTo(accommodationPlaceEntityDocumentList, options => options.ComparingByMembers<AccommodationPlaceEntityDocument>());
        }

        [Theory, AutoData]
        public async Task IncrementUsedAccommodationCapacity_ShouldIncrementCapacity(
            int placeId,
            AccommodationPlaceEntityDocument accommodationPlaceEntityDocument)
        {
            // Arrange
            accommodationPlaceEntityDocument.UsedAccommodationCapacity = 5;
            var expectedCapacityAfterIncrement = accommodationPlaceEntityDocument.UsedAccommodationCapacity + 1;
            _mockRepository.Setup(repo => repo.GetAsync(placeId))
                .ReturnsAsync(accommodationPlaceEntityDocument);
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
            AccommodationPlaceEntityDocument accommodationPlaceEntityDocument)
        {
            // Arrange
            accommodationPlaceEntityDocument.UsedAccommodationCapacity = 5;
            var expectedCapacityAfterDecrement = accommodationPlaceEntityDocument.UsedAccommodationCapacity - 1;
            _mockRepository.Setup(repo => repo.GetAsync(placeId))
                .ReturnsAsync(accommodationPlaceEntityDocument);
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

        [Theory, AutoData]
        public async Task ListAsync_ShouldReturnFilteredResults(
        Contracts.AccommodationPlaceEntity.FilterAccommodationPlaceEntity filter,
        FilterAccommodationPlaceEntityPagination pagination,
        List<AccommodationPlaceEntityDocument> accommodationPlaceEntityDocumentList)
        {
            // Arrange
            _mockRepository
                .Setup(repo => repo.ListAsync(It.IsAny<Repositories.Models.AccommodationPlaceEntity.FilterAccommodationPlaceEntity>()))
                .ReturnsAsync(accommodationPlaceEntityDocumentList);

            // Act
            var result = await _service.ListAsync(filter, pagination);

            // Assert
            result.Should().BeEquivalentTo(accommodationPlaceEntityDocumentList.Select(AccommodationPlaceEntityMapperExtension.Map));
            _mockRepository.Verify(repo => repo.ListAsync(It.IsAny<Repositories.Models.AccommodationPlaceEntity.FilterAccommodationPlaceEntity>()), Times.Once);
        }

        [Theory, AutoData]
        public async Task ListAsync_ShouldThrowResourceNotFoundException_WhenNoResults(
        Contracts.AccommodationPlaceEntity.FilterAccommodationPlaceEntity filter,
        FilterAccommodationPlaceEntityPagination pagination)
        {
            // Arrange
            _mockRepository
                .Setup(repo => repo.ListAsync(It.IsAny<Repositories.Models.AccommodationPlaceEntity.FilterAccommodationPlaceEntity>()))
                .ReturnsAsync(new List<AccommodationPlaceEntityDocument>());

            // Act & Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(() => _service.ListAsync(filter, pagination));
        }
    }
}