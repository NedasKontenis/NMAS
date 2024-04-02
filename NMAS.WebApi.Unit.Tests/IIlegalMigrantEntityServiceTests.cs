using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using NMAS.WebApi.Contracts.AccommodationPlaceEntity;
using NMAS.WebApi.Contracts.Enums;
using NMAS.WebApi.Contracts.Exceptions;
using NMAS.WebApi.Contracts.IllegalMigrantEntity;
using NMAS.WebApi.Repositories.IllegalMigrantEntity;
using NMAS.WebApi.Repositories.Models.IllegalMigrantEntity;
using NMAS.WebApi.Services.AccommodationPlaceEntityService;
using NMAS.WebApi.Services.Extensions;
using NMAS.WebApi.Services.IllegalMigrantEntity;
using NMAS.WebApi.Unit.Tests.CustomAutoDataFixture;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NMAS.WebApi.Unit.tests
{
    [Trait("Category", "Unit")]
    public class IllegalMigrantEntityServiceTests
    {
        private readonly Mock<IIllegalMigrantEntityRepository> _mockIllegalMigrantEntityRepository;
        private readonly Mock<IAccommodationPlaceEntityService> _mockAccommodationPlaceEntityService;
        private readonly IllegalMigrantEntityService _service;

        public IllegalMigrantEntityServiceTests()
        {
            _mockIllegalMigrantEntityRepository = new Mock<IIllegalMigrantEntityRepository>();
            _mockAccommodationPlaceEntityService = new Mock<IAccommodationPlaceEntityService>();
            _service = new IllegalMigrantEntityService(_mockIllegalMigrantEntityRepository.Object, _mockAccommodationPlaceEntityService.Object);
        }

        [Theory, AutoData]
        public async Task CreateAsync_ShouldReturnCreatedEntity(
            CreateIllegalMigrantEntity createIllegalMigrantEntity,
            int resourceId)
        {
            // Arrange
            _mockIllegalMigrantEntityRepository.Setup(r => r.CreateAsync(It.IsAny<IllegalMigrantEntityDocument>()))
                .ReturnsAsync(resourceId);

            // Act
            var result = await _service.CreateAsync(createIllegalMigrantEntity);

            // Assert
            Assert.Equal(resourceId, result.Id);
            _mockIllegalMigrantEntityRepository.Verify(r => r.CreateAsync(It.IsAny<IllegalMigrantEntityDocument>()), Times.Once);
        }

        [Theory, AutoData]
        public async Task GetAsync_ShouldReturnEntity_WhenEntityExists(
            int resourceId,
            IllegalMigrantEntityDocument illegalMigrantEntityDocument)
        {
            // Arrange
            _mockIllegalMigrantEntityRepository.Setup(r => r.GetAsync(resourceId))
                .ReturnsAsync(illegalMigrantEntityDocument);

            // Act
            var result = await _service.GetAsync(resourceId);

            // Assert
            Assert.NotNull(result);
            result.Should().BeEquivalentTo(illegalMigrantEntityDocument, options => options.ComparingByMembers<IllegalMigrantEntity>());
        }

        [Theory, AutoData]
        public async Task GetAsync_ShouldThrowResourceNotFoundException_WhenEntityDoesNotExist(
            int resourceId)
        {
            // Arrange
            _mockIllegalMigrantEntityRepository.Setup(repo => repo.GetAsync(resourceId))
                .ReturnsAsync((IllegalMigrantEntityDocument)null);

            // Act & Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(() => _service.GetAsync(resourceId));
        }

        [Theory, AutoData]
        public async Task UpdateAsync_ShouldUpdateEntity_WhenEntityExists(
            int resourceId,
            UpdateIllegalMigrantEntity updateIllegalMigrantEntity)
        {
            // Arrange
            _mockIllegalMigrantEntityRepository.Setup(r => r.GetAsync(resourceId)).ReturnsAsync(new IllegalMigrantEntityDocument());

            // Act
            await _service.UpdateAsync(resourceId, updateIllegalMigrantEntity);

            // Assert
            _mockIllegalMigrantEntityRepository.Verify(r => r.UpdateAsync(resourceId, It.IsAny<IllegalMigrantEntityDocument>()), Times.Once);
        }

        [Theory, AutoData]
        public async Task UpdateAsync_ShouldThrowResourceNotFoundException_WhenEntityDoesNotExist(
            int resourceId,
            UpdateIllegalMigrantEntity updateIllegalMigrantEntity)
        {
            // Arrange
            _mockIllegalMigrantEntityRepository.Setup(repo => repo.GetAsync(resourceId))
                .ReturnsAsync((IllegalMigrantEntityDocument)null);

            // Act & Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(() => _service.UpdateAsync(resourceId, updateIllegalMigrantEntity));

            _mockIllegalMigrantEntityRepository.Verify(repo => repo.GetAsync(resourceId), Times.Once);
            _mockIllegalMigrantEntityRepository.Verify(repo => repo.UpdateAsync(It.IsAny<int>(), It.IsAny<IllegalMigrantEntityDocument>()), Times.Never);
        }

        [Theory, AutoData]
        public async Task UpdateAsync_ShouldInvokeDecrementUsedAccommodationCapacity_WhenAccommodationPlaceIdChanges(
            int resourceId,
            IllegalMigrantEntityDocument illegalMigrantEntityDocument,
            UpdateIllegalMigrantEntity updateIllegalMigrantEntity)
        {
            // Arrange
            illegalMigrantEntityDocument.AccommodationPlaceId = 1;
            updateIllegalMigrantEntity.AccommodationPlaceId = 2;

            _mockIllegalMigrantEntityRepository.Setup(repo => repo.GetAsync(resourceId))
                .ReturnsAsync(illegalMigrantEntityDocument);

            // Act
            await _service.UpdateAsync(resourceId, updateIllegalMigrantEntity);

            // Assert
            _mockAccommodationPlaceEntityService.Verify(service => service.DecrementUsedAccommodationCapacity(illegalMigrantEntityDocument.AccommodationPlaceId.Value), Times.Once);
        }

        [Theory, AutoData]
        public async Task UpdateAsync_ShouldInvokeIncrementUsedAccommodationCapacity_WhenAccommodationPlaceIdIsAddedOrChanged(
            int resourceId,
            IllegalMigrantEntityDocument illegalMigrantEntityDocument,
            UpdateIllegalMigrantEntity updateIllegalMigrantEntity)
        {
            // Arrange
            illegalMigrantEntityDocument.AccommodationPlaceId = null;

            _mockIllegalMigrantEntityRepository.Setup(repo => repo.GetAsync(resourceId))
                .ReturnsAsync(illegalMigrantEntityDocument);

            // Act
            await _service.UpdateAsync(resourceId, updateIllegalMigrantEntity);

            // Assert
            _mockAccommodationPlaceEntityService.Verify(service => service.IncrementUsedAccommodationCapacity(updateIllegalMigrantEntity.AccommodationPlaceId.Value), Times.Once);
        }

        [Theory, AutoData]
        public async Task DeleteAsync_ShouldDeleteEntity_WhenEntityExists(
            int resourceId)
        {
            // Arrange
            _mockIllegalMigrantEntityRepository.Setup(r => r.GetAsync(resourceId))
                .ReturnsAsync(new IllegalMigrantEntityDocument());

            // Act
            await _service.DeleteAsync(resourceId);

            // Assert
            _mockIllegalMigrantEntityRepository.Verify(r => r.DeleteAsync(resourceId), Times.Once);
        }

        [Theory, AutoData]
        public async Task DeleteAsync_ShouldInvokeDecrementUsedAccommodationCapacity_WhenEntityHasAccommodationPlaceId(
            int resourceId,
            IllegalMigrantEntityDocument illegalMigrantEntityDocument)
        {
            // Arrange
            _mockIllegalMigrantEntityRepository.Setup(repo => repo.GetAsync(resourceId))
                .ReturnsAsync(illegalMigrantEntityDocument);

            // Act
            await _service.DeleteAsync(resourceId);

            // Assert
            _mockAccommodationPlaceEntityService.Verify(service => service.DecrementUsedAccommodationCapacity(illegalMigrantEntityDocument.AccommodationPlaceId.Value), Times.Once);
            _mockIllegalMigrantEntityRepository.Verify(repo => repo.DeleteAsync(resourceId), Times.Once);
        }

        [Theory, AutoData]
        public async Task AssignAsync_ShouldThrowBadRequestException_WhenMigrantAlreadyHasAccommodationPlaceId(
            int resourceId,
            IllegalMigrantEntityDocument illegalMigrantEntityDocument)
        {
            // Arrange
            illegalMigrantEntityDocument.AccommodationPlaceId = 123;
            _mockIllegalMigrantEntityRepository.Setup(repo => repo.GetAsync(resourceId))
                .ReturnsAsync(illegalMigrantEntityDocument);

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _service.AssignAsync(resourceId));

            _mockIllegalMigrantEntityRepository.Verify(repo => repo.GetAsync(resourceId), Times.Once);
            _mockIllegalMigrantEntityRepository.Verify(repo => repo.UpdateAsync(It.IsAny<int>(), It.IsAny<IllegalMigrantEntityDocument>()), Times.Never);
            _mockAccommodationPlaceEntityService.Verify(service => service.IncrementUsedAccommodationCapacity(It.IsAny<int>()), Times.Never);
        }

        [Theory]
        [InlineData(1)]
        public async Task AssignAsync_ShouldAssignToCompatibleAccommodation(
            int resourceId)
        {
            // Arrange
            var places = new List<AccommodationPlaceEntity>
            {
                new AccommodationPlaceEntity { Id = 1, AccommodationCapacity = 1000, UsedAccommodationCapacity = 1000 }, // Fully occupied
                new AccommodationPlaceEntity { Id = 2, AccommodationCapacity = 500, UsedAccommodationCapacity = 250 },
            };

            var migrantToAssign = new IllegalMigrantEntityDocument
            {
                AccommodationPlaceId = null,
            };

            _mockIllegalMigrantEntityRepository.Setup(r => r.GetAsync(resourceId)).ReturnsAsync(migrantToAssign);
            _mockIllegalMigrantEntityRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<IllegalMigrantEntityDocument>());
            _mockAccommodationPlaceEntityService.Setup(s => s.GetAllAccommodationPlacesAsync()).ReturnsAsync(places);

            // Act
            await _service.AssignAsync(resourceId);

            // Assert
            _mockIllegalMigrantEntityRepository.Verify(r => r.UpdateAsync(resourceId, It.Is<IllegalMigrantEntityDocument>(m => m.AccommodationPlaceId.HasValue && m.AccommodationPlaceId.Value == 2)), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        public async Task AssignAsync_ShouldThrowBadRequestException_WhenAccommodationCapacityExceedsLimit(
            int resourceId)
        {
            // Arrange
            var places = new List<AccommodationPlaceEntity>
            {
                new AccommodationPlaceEntity { Id = 1, AccommodationCapacity = 1000, UsedAccommodationCapacity = 1000 }, // Fully occupied
                new AccommodationPlaceEntity { Id = 2, AccommodationCapacity = 500, UsedAccommodationCapacity = 500 }, // Fully occupied
            };

            var migrantToAssign = new IllegalMigrantEntityDocument
            {
                AccommodationPlaceId = null,
            };

            _mockIllegalMigrantEntityRepository.Setup(r => r.GetAsync(resourceId)).ReturnsAsync(migrantToAssign);
            _mockAccommodationPlaceEntityService.Setup(s => s.GetAllAccommodationPlacesAsync()).ReturnsAsync(places);

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _service.AssignAsync(resourceId));

            _mockIllegalMigrantEntityRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<IllegalMigrantEntityDocument>()), Times.Never);
        }

        [Theory]
        [InlineData(1)]
        public async Task AssignAsync_ShouldThrowBadRequestException_WhenNoReligiouslyCompatibleAccommodationFound(
            int resourceId)
        {
            // Arrange
            var places = new List<AccommodationPlaceEntity>
            {
                new AccommodationPlaceEntity { Id = 1, AccommodationCapacity = 1000, UsedAccommodationCapacity = 500 },
                new AccommodationPlaceEntity { Id = 2, AccommodationCapacity = 500, UsedAccommodationCapacity = 250 },
            };

            var migrantToAssign = new IllegalMigrantEntityDocument
            {
                AccommodationPlaceId = null,
                Religion = IllegalMigrantReligion.Sunni.ToString()
            };

            var allMigrants = new List<IllegalMigrantEntityDocument>
            {
                new IllegalMigrantEntityDocument { Id = 1, AccommodationPlaceId = 1, Religion = IllegalMigrantReligion.Shia.ToString()},
                new IllegalMigrantEntityDocument { Id = 2, AccommodationPlaceId = 2, Religion = IllegalMigrantReligion.Shia.ToString()},
            };

            _mockAccommodationPlaceEntityService.Setup(s => s.GetAllAccommodationPlacesAsync()).ReturnsAsync(places);
            _mockIllegalMigrantEntityRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(allMigrants);
            _mockIllegalMigrantEntityRepository.Setup(r => r.GetAsync(resourceId)).ReturnsAsync(migrantToAssign);

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _service.AssignAsync(resourceId));

            _mockIllegalMigrantEntityRepository.Verify(r => r.GetAsync(resourceId), Times.Once);
            _mockIllegalMigrantEntityRepository.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<IllegalMigrantEntityDocument>()), Times.Never);
            _mockAccommodationPlaceEntityService.Verify(s => s.IncrementUsedAccommodationCapacity(It.IsAny<int>()), Times.Never);
        }

        [Theory, CustomAutoData]
        public async Task ListAsync_ShouldReturnFilteredResults(
            Contracts.IllegalMigrantEntity.FilterIllegalMigrantEntity filter,
            FilterIllegalMigrantEntityPagination pagination,
            List<IllegalMigrantEntityDocument> illegalMigrantEntityDocumentList)
        {
            _mockIllegalMigrantEntityRepository
                .Setup(repo => repo.ListAsync(It.IsAny<Repositories.Models.IllegalMigrantEntity.FilterIllegalMigrantEntity>()))
                .ReturnsAsync(illegalMigrantEntityDocumentList);

            // Act
            var result = await _service.ListAsync(filter, pagination);

            // Assert
            Assert.NotEmpty(result);
            result.Should().BeEquivalentTo(illegalMigrantEntityDocumentList.Select(IllegalMigrantEntityMapperExtension.Map));
            _mockIllegalMigrantEntityRepository.Verify(repo => repo.ListAsync(It.IsAny<Repositories.Models.IllegalMigrantEntity.FilterIllegalMigrantEntity>()), Times.Once);
        }

        [Theory, CustomAutoData]
        public async Task ListAsync_ShouldThrowResourceNotFoundException_WhenNoResults(
            Contracts.IllegalMigrantEntity.FilterIllegalMigrantEntity filter,
            FilterIllegalMigrantEntityPagination pagination)
        {
            // Arrange
            _mockIllegalMigrantEntityRepository
                .Setup(repo => repo.ListAsync(It.IsAny<Repositories.Models.IllegalMigrantEntity.FilterIllegalMigrantEntity>()))
                .ReturnsAsync(new List<IllegalMigrantEntityDocument>());

            // Act & Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(() => _service.ListAsync(filter, pagination));
        }
    }
}
