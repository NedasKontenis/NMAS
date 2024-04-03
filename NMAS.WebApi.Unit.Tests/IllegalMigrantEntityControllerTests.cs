using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NMAS.WebApi.Contracts.IllegalMigrantEntity;
using NMAS.WebApi.Host.Controllers;
using NMAS.WebApi.Services.IllegalMigrantEntity;
using System.Threading.Tasks;
using Xunit;

namespace NMAS.WebApi.Unit.Tests
{
    [Trait("Category", "Unit")]
    public class IllegalMigrantEntityControllerTests
    {
        private readonly Mock<IIllegalMigrantEntityService> _mockIllegalMigrantEntityService;
        private readonly IllegalMigrantEntityController _controller;

        public IllegalMigrantEntityControllerTests()
        {
            _mockIllegalMigrantEntityService = new Mock<IIllegalMigrantEntityService>();
            _controller = new IllegalMigrantEntityController(_mockIllegalMigrantEntityService.Object);
        }

        [Theory, AutoData]
        public async Task GetById_Returns200AndIllegalMigrant_WhenMigrantExists(
            int resourceId,
            IllegalMigrantEntity illegalMigrantEntity)
        {
            // Arrange
            illegalMigrantEntity.Id = resourceId;

            _mockIllegalMigrantEntityService.Setup(service => service.GetAsync(resourceId))
                .ReturnsAsync(illegalMigrantEntity);

            // Act
            var result = await _controller.Get(resourceId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedMigrant = Assert.IsType<IllegalMigrantEntity>(okResult.Value);
            Assert.Equal(resourceId, returnedMigrant.Id);
        }
    }
}
