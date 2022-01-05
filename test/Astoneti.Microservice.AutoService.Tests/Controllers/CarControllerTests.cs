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
            _mockCarService = new Mock<ICarService>(MockBehavior.Strict);

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

            _mockCarService
                .Setup(x => x.GetList())
                .Returns(dtos);

            var expectedResultValue = _mapper.Map<IList<CarModel>>(dtos);

            // Act
            var result = _controller.GetList();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);

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

            _mockCarService
                .Setup(x => x.Get(id))
                .Returns(dto);

            var expectedResultValue = _mapper.Map<CarModel>(dto);

            // Act
            var result = _controller.Get(id);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);

            var resultValue = Assert.IsType<CarModel>(okObjectResult.Value);

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
        }

        [Fact]
        public void Post_ValidObjectPassed_ReturnedResponseHasCreatedItem()
        {
            // Arrange           
            var model = new CarPostModel
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
                .Setup(x => x.Add(model))
                .Returns(dto);

            var expectedResultValue = _mapper.Map<CarModel>(dto);

            // Act
            var result = _controller.Post(model);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);

            Assert.Equal("Get", createdAtActionResult.ActionName);
            Assert.Null(createdAtActionResult.ControllerName);
            var idRouteValue = Assert.Single(createdAtActionResult.RouteValues);
            Assert.Equal("id", idRouteValue.Key);
            Assert.Equal(expectedResultValue.Id, idRouteValue.Value);

            var resultValue = Assert.IsType<CarModel>(createdAtActionResult.Value);

            resultValue
                .Should()
                .BeEquivalentTo(expectedResultValue);
        }

        [Fact]
        public void Put_WhenModelNotValid_Should_ReturnBadRequest()
        {
            // Arrange
            const int id = 1;

            var model = new CarPutModel()
            {
                Id = 2
            };

            // Act
            var result = _controller.Put(id, model);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Put_WhenItemNotExists_Should_ReturnNotFound()
        {
            // Arrange
            const int id = 1;

            var model = new CarPutModel()
            {
                Id = id,
                CarBrand = "Test New Car",
            };

            _mockCarService
                .Setup(x => x.Edit(model))
                .Returns(() => null);

            // Act
            var result = _controller.Put(id, model);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Put_Should_UpdateItem()
        {
            // Arrange
            const int id = 1;

            var model = new CarPutModel()
            {
                Id = id,
                CarBrand = "Test New Car",
            };

            var dto = new CarDto()
            {
                Id = id,
                CarBrand = "Test Car",
                Model = "TestModel",
            };

            _mockCarService
                .Setup(x => x.Edit(model))
                .Returns(dto);

            // Act
            var result = _controller.Put(id, model);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
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
            Assert.IsType<NotFoundResult>(result);
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
            Assert.IsType<OkResult>(result);
        }
    }
}
