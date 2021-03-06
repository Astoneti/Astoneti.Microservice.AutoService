using Astoneti.Microservice.AutoService.Business.Contracts;
using Astoneti.Microservice.AutoService.Business.Models;
using Astoneti.Microservice.AutoService.Controllers;
using Astoneti.Microservice.AutoService.Models.Owner;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Astoneti.Microservice.AutoService.Tests.Controllers
{
    public class OwnerControllerTests
    {
        private readonly Mock<IOwnerService> _mockOwnerService;
        private readonly IMapper _mapper;

        private readonly OwnerController _controller;

        public OwnerControllerTests()
        {
            _mockOwnerService = new Mock<IOwnerService>();

            _mapper = new MapperConfiguration(
                    config => config.AddMaps(
                        typeof(Startup).Assembly
                    )
                )
                .CreateMapper();

            _controller = new OwnerController(
                _mockOwnerService.Object,
                _mapper
            );
        }

        [Fact]
        public void GetList_WhenParametersIsValid_Should_ReturnExpectedResult()
        {
            // Arrange
            var ownersDto = new List<OwnerDto>()
            {
                new OwnerDto()
                {
                    Id = 1,
                   Name = "Test Owner"
                }
            };

            _mockOwnerService
                .Setup(x => x.GetList())
                .Returns(ownersDto);

            var expectedResultValue = _mapper.Map<IList<OwnerModel>>(ownersDto);

            // Act
            var result = _controller.GetList();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);

            var resultValue = Assert.IsAssignableFrom<IList<OwnerModel>>(okObjectResult.Value);

            resultValue
                .Should()
                .BeEquivalentTo(expectedResultValue);
        }

        [Fact]
        public void Get_WhenItemExists_Should_ReturnItemById()
        {
            // Arrange
            const int id = 1;

            var carsDto = new List<CarDto>()
            {
                new CarDto()
                {
                    Id = 1,
                    CarBrand = "Test Car"
                }
            };
            
            var ownerDto = new OwnerDto()
            {
                Cars = carsDto,
                Id = id,
                Name = "Test Owner"
            };

            _mockOwnerService
                .Setup(x => x.Get(id))
                .Returns(ownerDto);

            var expectedResultValue = _mapper.Map<OwnerDto>(ownerDto);

            // Act
            var result = _controller.Get(id);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);

            var resultValue = Assert.IsType<OwnerModel>(okObjectResult.Value);

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

            _mockOwnerService
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
            var model = new OwnerPostModel
            {
                Name = "Test Owner",
            };

            var dto = new OwnerDto
            {
                Name = "Test Owner",
            };

            _mockOwnerService
                .Setup(x => x.Add(model))
                .Returns(dto);

            var expectedResultValue = _mapper.Map<OwnerModel>(dto);

            // Act
            var result = _controller.Post(model);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);

            Assert.Equal("Get", createdAtActionResult.ActionName);
            Assert.Null(createdAtActionResult.ControllerName);
            var idRouteValue = Assert.Single(createdAtActionResult.RouteValues);
            Assert.Equal("id", idRouteValue.Key);
            Assert.Equal(expectedResultValue.Id, idRouteValue.Value);

            var resultValue = Assert.IsType<OwnerModel>(createdAtActionResult.Value);

            resultValue
                .Should()
                .BeEquivalentTo(expectedResultValue);
        }

        [Fact]
        public void Put_WhenModelNotValid_Should_ReturnBadRequest()
        {
            // Arrange
            const int id = 1;

            var model = new OwnerPutModel()
            {
                CarId = 2,
                Name = "Test New Owner",
            };

            var dto = new OwnerDto()
            {
                Id = id,
                Name = "Test Owner",
            };

            _mockOwnerService
                .Setup(_ => _.Edit(model))
                .Returns(dto);


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

            var model = new OwnerPutModel()
            {
                CarId = id,
                Name = "Test New Owner",
            };

            _mockOwnerService
                .Setup(_ => _.Edit(model))
                .Returns(() => null);

            // Act
            var result = _controller.Put(id, model);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Put_Should_UpdateCreatedItem()
        {
            const int id =1;
            // Arrange
            var model = new OwnerPutModel
            {
                CarId = id,
                Name = "Test Owner",
            };

            var dto = new OwnerDto()
            {
                Id = id,
                Name = "Test Owner",
            };

            _mockOwnerService
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

            _mockOwnerService
                .Setup(x => x.Get(id))
                .Returns(() => null);

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

            _mockOwnerService
                .Setup(x => x.Delete(id))
                .Returns(true);

            // Act
            var result = _controller.Delete(id);

            // Assert
            Assert.IsType<OkResult>(result);
        }
    }
}
