using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NMAS.WebApi.Contracts.AccommodationPlaceEntity;
using NMAS.WebApi.Host.Controllers;
using NMAS.WebApi.Services.AccommodationPlaceEntityService;
using System.Threading.Tasks;
using Xunit;
using NMAS.WebApi.Contracts.AccomodationPlaceEntity;

namespace NMAS.WebApi.Unit.Tests
{
    [Trait("Category", "Unit")]
    public class AccommodationPlaceEntityControllerTests
    {

        private readonly AccommodationPlaceEntityController _controller;
        private readonly Mock<IAccommodationPlaceEntityService> _mockAccommodationPlaceEntityService;

        public AccommodationPlaceEntityControllerTests()
        {
            _mockAccommodationPlaceEntityService = new Mock<IAccommodationPlaceEntityService>();
            _controller = new AccommodationPlaceEntityController(_mockAccommodationPlaceEntityService.Object);
        }

        [Theory, AutoData]
        public async Task GetById_Returns200AndAccommodationPlace_WhenAccommodationPlaceExists(
            int resourceId,
            AccommodationPlaceEntity AccommodationPlaceEntity)
        {
            // Arrang
            AccommodationPlaceEntity.Id = resourceId;

            _mockAccommodationPlaceEntityService.Setup(service => service.GetAsync(resourceId))
                .ReturnsAsync(AccommodationPlaceEntity);

            // Act
            var result = await _controller.Get(resourceId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedAccommodationPlace = Assert.IsType<AccommodationPlaceEntity>(okResult.Value);
            Assert.Equal(resourceId, returnedAccommodationPlace.Id);
        }

        [Fact]
        public async Task Post_ValidRequest_CreatesAccommodationPlaceAndReturns201()
        {
            // Arrange
            var serviceMock = new Mock<IAccommodationPlaceEntityService>();
            var controller = new AccommodationPlaceEntityController(serviceMock.Object);
            var newAccommodationPlace = new CreateAccommodationPlaceEntity
            {
                WorkerId = 1,
                PlaceName = "Pabrades migrantu centras",
                Adress = "Vilniaus g. 100, Pabradė",
                AccommodationCapacity = 300,
                UsedAccommodationCapacity = 126,
                CompanyCode = "123456",
                ContactPhone = "038753401"
                //Populate with valid data

            };
            var expectedResponse = new AccommodationPlaceEntityCreated { Id = 1 };
            serviceMock.Setup(x => x.CreateAsync(It.IsAny<CreateAccommodationPlaceEntity>()))
                       .ReturnsAsync(expectedResponse);

            // Act
            var result = await controller.Create(newAccommodationPlace);

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
            var serviceMock = new Mock<IAccommodationPlaceEntityService>();
            var controller = new AccommodationPlaceEntityController(serviceMock.Object);
            controller.ModelState.AddModelError("PlaceIdentityCode", "Required"); // Simulate an invalid model state

            var invalidRequest = new CreateAccommodationPlaceEntity
            {
                WorkerId = 1,
                PlaceName = "Pabrades migrantu centras",
                Adress = "Vilniaus g. 100, Pabradė",
                AccommodationCapacity = 300,
                UsedAccommodationCapacity = 126,
                CompanyCode = "123456",
                ContactPhone = "038753401"
                // PlaceName is not specified
            };

            // Act
            var response = await controller.Create(invalidRequest);

            // Assert
            Assert.IsType<BadRequestObjectResult>(response);
            serviceMock.Verify(x => x.CreateAsync(It.IsAny<CreateAccommodationPlaceEntity>()), Times.Never); // Ensure service is not called
        }

        [Fact]
        public async Task Put_ValidRequest_UpdateAccommodationPlaceAndReturns204()
        {
            // Arrange
            var newAccommodationPlace = new UpdateAccommodationPlaceEntity
            {
                WorkerId = 1,
                PlaceName = "Pabrades migrantu centras",
                Adress = "Vilniaus g. 100, Pabradė",
                AccommodationCapacity = 300,
                UsedAccommodationCapacity = 126,
                CompanyCode = "123456",
                ContactPhone = "038753401"
                // Populate with valid data
            };
            int id = 1;
            _mockAccommodationPlaceEntityService.Setup(x => x.UpdateAsync(id, newAccommodationPlace)).Returns(Task.CompletedTask);

            //act
            var result = await _controller.Update(id, newAccommodationPlace);

            //Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Put_InvalidRequest_UpdateAccommodationPlaceAndReturns400()
        {
            // Arrange
            var newAccommodationPlace = new UpdateAccommodationPlaceEntity
            {
                WorkerId = 1,
                Adress = "Vilniaus g. 100, Pabradė",
                AccommodationCapacity = 300,
                UsedAccommodationCapacity = 126,
                CompanyCode = "123456",
                ContactPhone = "038753401"
                // Populate with invalid data
            };
            int id = 1;
            _controller.ModelState.AddModelError("PlaceName", "Required");
            //act
            var result = await _controller.Update(id, newAccommodationPlace);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Theory, AutoData]
        public async Task DeleteByPlaceName_Returns204NoContent_WhenAccommodationPlaceExists(int resourceId, AccommodationPlaceEntity accommodationPlaceEntity)
        {
            // Arrange
            accommodationPlaceEntity.Id = resourceId;
            var mockService = new Mock<IAccommodationPlaceEntityService>();
            mockService.Setup(service => service.DeleteAsync(resourceId)).Returns(Task.FromResult(true));
            var controller = new AccommodationPlaceEntityController(mockService.Object);

            // Act
            var result = await controller.Delete(resourceId);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContentResult.StatusCode);
            mockService.Verify(service => service.DeleteAsync(resourceId), Times.Once);
        }
    }
}
