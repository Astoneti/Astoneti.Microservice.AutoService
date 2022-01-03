using Astoneti.Microservice.AutoService.Business.Contracts;
using Astoneti.Microservice.AutoService.Business.Models;
using Astoneti.Microservice.AutoService.Contollers;
using Astoneti.Microservice.AutoService.Models.Car;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;

namespace Astoneti.Microservice.AutoService.Tests.Controllers
{
    public class CarControllerTests
    {
        private readonly Mock<ICarService> _mockCarService;
        private readonly IMapper _mapper;
        private readonly CarController _controller;

        public CarControllerTests()
        {
            _mockCarService = new Mock<ICarService>();

            _mapper = new MapperConfiguration(
                    config => config.AddMaps(
                        typeof(Startup).Assembly
                    )
                )
                .CreateMapper();

            _controller = new CarController(
                _mockCarService.Object,
                _mapper
            );
        }

        [Fact]
        public void GetList_WhenParametersIsValid_Should_ReturnExpectedResult()
        {
            // Arrange
            var dtos = new List<CarDto>()
            {
                new CarDto()
                {
                    Id = 1,
                    CarBrand = "Mazda",
                    Model = "CX-7",
                    LicensePlate = "5757 IX-7"
                }
            };

            var expectedResultValue = _mapper.Map<IList<CarModel>>(dtos);

            _mockCarService
                .Setup(x => x.GetList())
                .Returns(dtos);

            // Act
            var result = _controller.GetList();

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);

            var resultValue = Assert.IsAssignableFrom<IList<CarModel>>(okObjectResult.Value);

            resultValue
                .Should()
                .BeEquivalentTo(expectedResultValue);
        }

        [Fact]
        public void Get_WhenItemExists_Should_ReturnItemById()
        {
            // Arrange
            const int id = 1;

            var dto = new CarDto()
            {
                Id = id,
                CarBrand = "Mazda",
                Model = "CX-7",
            };

            var expectedResultValue = _mapper.Map<CarModel>(dto);

            _mockCarService
                .Setup(x => x.Get(id))
                .Returns(dto);

            // Act
            var result = _controller.Get(id);

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);

            var resultValue = Assert.IsAssignableFrom<CarModel>(okObjectResult.Value);

            Assert.IsType<OkObjectResult>(result as OkObjectResult);

            resultValue
                .Should()
                .BeEquivalentTo(expectedResultValue);
        }

        [Fact]
        public void Get_WhenItemNotExists_Should_ReturnNotFound()
        {
            // Arrange
            const int id = 1;

            _mockCarService
                .Setup(_ => _.Get(id))
                .Returns(() => null);

            // Act
            var result = _controller.Get(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.NotNull(result);
        }

        [Fact]
        public void Post_ValidObjectPassed_ReturnedResponseHasCreatedItem()
        {
            // Arrange           
            var postModel = new CarPostModel
            {
                CarBrand = "Test Car",
            };

            var dto = new CarDto()
            {
                Id = 1,
                CarBrand = "Test Car",
                Model = "TestModel",
            };

            _mockCarService
                .Setup(x => x.Add(postModel))
                .Returns(dto);

            var expectedResultValue = _mapper.Map<CarModel>(dto);

            // Act
            var result = _controller.Post(postModel);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);

            var resultValue = Assert.IsType<CarModel>(createdAtActionResult.Value);

            resultValue
                .Should()
                .BeEquivalentTo(expectedResultValue);
        }

        [Fact]
        public void Put_Should_UpdateCreatedItem()
        {
            // Arrange
            var putModel = new CarPutModel
            {
                CarBrand = "Test Car",
            };

            var dto = new CarDto()
            {
                Id = 1,
                CarBrand = "Test Car",
                Model = "TestModel",
            };

            _mockCarService
                .Setup(x => x.Edit(putModel))
                .Returns(dto);

            var expectedResultValue = _mapper.Map<CarModel>(dto);

            // Act
            var result = _controller.Put(1, putModel);

            // Assert
            var createdResponse = Assert.IsType<OkObjectResult>(result);

            var resultValue = Assert.IsType<CarModel>(createdResponse.Value);

            resultValue.Should().BeEquivalentTo(expectedResultValue);
        }

        [Fact]
        public void Delete_WhenItemIsNotExists_Should_ReturnNotFound()
        {
            // Arrange
            const int id = 1;

            _mockCarService
                .Setup(x => x.Delete(id))
                .Returns(() => false);

            // Act
            var result = _controller.Delete(id);

            // Assert
            Assert.IsAssignableFrom<NotFoundResult>(result);
            Assert.NotNull(result);
        }

        [Fact]
        public void Delete_Should_RemovesItem()
        {
            // Arrange
            const int id = 1;

            _mockCarService
                .Setup(x => x.Delete(id))
                .Returns(true);

            // Act
            var result = _controller.Delete(id);

            // Assert
            Assert.IsAssignableFrom<StatusCodeResult>(result);
            Assert.IsType<OkResult>(result);
        }
    }
}
