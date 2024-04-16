using AutoFixture.Xunit2;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NMAS.WebApi.Contracts.Exceptions;
using NMAS.WebApi.Contracts.IllegalMigrantEntity;
using NMAS.WebApi.Host.Controllers;
using NMAS.WebApi.Repositories.Models.IllegalMigrantEntity;
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

        [Fact]
        public async Task Post_ValidRequest_CreatesMigrantAndReturns201()
        {
            // Arrange
            var serviceMock = new Mock<IIllegalMigrantEntityService>();
            var controller = new IllegalMigrantEntityController(serviceMock.Object);
            var newMigrant = new CreateIllegalMigrantEntity
            {
                PersonalIdentityCode = "1516516",
                FirstName = "Amar",
                LastName = "Ali",
                Gender = "Male",
                DateOfBirth = new System.DateTime(1990, 1, 1),
                OriginCountry = "Iran",
                Religion = "Christianity"

                // Populate with valid data
            };
            var expectedResponse = new IllegalMigrantEntityCreated { Id = 1 };
            serviceMock.Setup(x => x.CreateAsync(It.IsAny<CreateIllegalMigrantEntity>()))
                       .ReturnsAsync(expectedResponse);

            // Act
            var result = await controller.Create(newMigrant);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(StatusCodes.Status201Created, createdAtActionResult.StatusCode);
            Assert.Equal(expectedResponse, createdAtActionResult.Value);
            Assert.Equal(expectedResponse.Id, createdAtActionResult.RouteValues["id"]);
        }

        [Fact]
        public async Task Post_InvalidRequest_Returns400BadRequest()
        {
            // Arrange
            var serviceMock = new Mock<IIllegalMigrantEntityService>();
            var controller = new IllegalMigrantEntityController(serviceMock.Object);
            controller.ModelState.AddModelError("PersonalIdentityCode", "Required"); // Simulate an invalid model state

            var invalidRequest = new CreateIllegalMigrantEntity
            {
                PersonalIdentityCode = "1516516",
                FirstName = "Amar",
                LastName = "Ali",
                Gender = "Male",
                DateOfBirth = new System.DateTime(1990, 1, 1),
                OriginCountry = "Iran",
                Religion = "Christianity"
                // PersonalIdentityCode is not specified
            };

            // Act
            var response = await controller.Create(invalidRequest);

            // Assert
            Assert.IsType<BadRequestObjectResult>(response);
            serviceMock.Verify(x => x.CreateAsync(It.IsAny<CreateIllegalMigrantEntity>()), Times.Never); // Ensure service is not called
        }
 introducing-unit-tests-nmas-22
        [Fact]
        public async Task Put_ValidRequest_UpdateMigrantAndReturns204()
        {
            // Arrange
            var newMigrant = new UpdateIllegalMigrantEntity
            {
                PersonalIdentityCode = "1516516",
                FirstName = "Amar",
                LastName = "Ali",
                Gender = "Male",
                DateOfBirth = new System.DateTime(1990, 1, 1),
                OriginCountry = "Iran",
                Religion = "Christianity"

                // Populate with valid data
            };
            int id = 1;
            _mockIllegalMigrantEntityService.Setup(x => x.UpdateAsync(id, newMigrant)).Returns(Task.CompletedTask);

            //act
            var result = await _controller.Update(id, newMigrant);

            //Assert
            Assert.IsType<NoContentResult>(result);
        }
        [Fact]
        public async Task Put_InvalidRequest_UpdateMigrantAndReturns400()
        {
            // Arrange
            var newMigrant = new UpdateIllegalMigrantEntity
            {
                PersonalIdentityCode = "1516516",
                LastName = "Ali",
                Gender = "Male",
                DateOfBirth = new System.DateTime(1990, 1, 1),
                OriginCountry = "Iran",
                Religion = "Christianity"

                // Populate with invalid data
            };
            int id = 1;
            _controller.ModelState.AddModelError("FirstName", "Required");
            //act
            var result = await _controller.Update(id, newMigrant);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }


        [Theory, AutoData]
        public async Task DeleteById_Returns204NoContent_WhenMigrantExists(int resourceId, IllegalMigrantEntity illegalMigrantEntity)
        {
            // Arrange
            illegalMigrantEntity.Id = resourceId;
            var mockService = new Mock<IIllegalMigrantEntityService>();
            mockService.Setup(service => service.DeleteAsync(resourceId)).Returns(Task.FromResult(true));
            var controller = new IllegalMigrantEntityController(mockService.Object);

            // Act
            var result = await controller.Delete(resourceId);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContentResult.StatusCode);
            mockService.Verify(service => service.DeleteAsync(resourceId), Times.Once);
        }


    }
}
