using Astoneti.Microservice.AutoService.Business.Contracts;
using Astoneti.Microservice.AutoService.Business.Models;
using Astoneti.Microservice.AutoService.Controllers;
using Astoneti.Microservice.AutoService.Models.Car;
using Astoneti.Microservice.AutoService.Models.Owner;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Astoneti.Microservice.AutoService.Tests.Controllers
{
    public class SearchControllerTests
    {
        private readonly Mock<IOwnerService> _mockOwnerService;
        private readonly Mock<ICarService> _mockCarService;
        private readonly IMapper _mapper;
        private SearchController _controller;

        public SearchControllerTests()
        {
            _mockOwnerService = new Mock<IOwnerService>();
            _mockCarService = new Mock<ICarService>();
            _mapper = new MapperConfiguration(
                    config => config.AddMaps(
                        typeof(Startup).Assembly
                    )
                )
                .CreateMapper();

            _controller = new SearchController(
                _mockCarService.Object,
                _mockOwnerService.Object,
                _mapper
            );
        }

        [Fact]
        public void Get_WhenParametersIsValid_Should_ReturnExpectedResultOfOwnersWithCarsList()
        {
            const int id = 1;

            List<CarDto> carsDto = new()
            {
                new CarDto()
                {
                    Id = 1,
                    CarBrand = "Test Car"
                }
            };

            List<OwnerDto> listDto = new()
            {
                new OwnerDto()
                {
                    Cars = carsDto,
                    Id = id,
                    Name = "Test Owner"
                }
            };

            var expectedResultValue = _mapper.Map<IList<OwnerModel>>(listDto);

            _mockOwnerService
                .Setup(x => x.GetCarsList())
                .Returns(listDto);

            // Act
            var result = _controller.GetOwnersWithCarsList();

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);

            var resultValue = Assert.IsAssignableFrom<IList<OwnerModel>>(okObjectResult.Value);

            resultValue
                .Should()
                .BeEquivalentTo(expectedResultValue);
        }

        [Fact]
        public void Get_WhenParametersIsValid_Should_ReturnExpectedResultOfOwnerWhereMoreThenOneCar()
        {
            const int id = 1;

            List<CarDto> carsDto = new()
            {
                new CarDto()
                {
                    Id = 1,
                    CarBrand = "Test Car",
                },

                new CarDto()
                {
                    Id = 2,
                    CarBrand = "Test Car 2",
                }
            };

            List<OwnerDto> listDto = new()
            {
                new OwnerDto()
                {
                    Cars = carsDto,
                    Id = id,
                    Name = "Test Owner"
                }
            };

            var expectedResultValue = _mapper.Map<IList<OwnerModel>>(listDto);

            _mockOwnerService
                .Setup(x => x.GetOwnerWhereMoreThenOneCar())
                .Returns(listDto);

            // Act
            var result = _controller.GetOwnerWhereMoreThenOneCar();

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);

            var resultValue = Assert.IsAssignableFrom<IList<OwnerModel>>(okObjectResult.Value);

            resultValue
                .Should()
                .BeEquivalentTo(expectedResultValue);
        }

        [Fact]
        public void Get_WhenParametersIsValid_Should_ReturnExpectedResultOfCarsWhithOwnersList()
        {
            const int id = 1;

            var ownerDto = new OwnerDto()
            {
                Id = 1,
                Name = "Test Owner"
            };
            
            List<CarDto> listDto = new()
            {
                new CarDto()
                {
                    
                    Id = id,
                    Owner = ownerDto,
                    CarBrand = "Test Car"
                }
            };

            var expectedResultValue = _mapper.Map<IList<CarModel>>(listDto);

            _mockCarService
                .Setup(x => x.GetOwnersList())
                .Returns(listDto);

            // Act
            var result = _controller.GetCarsWhithOwnersList();

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);

            var resultValue = Assert.IsAssignableFrom<IList<CarModel>>(okObjectResult.Value);

            resultValue
                .Should()
                .BeEquivalentTo(expectedResultValue);
        }

        [Fact]
        public void Get_WhenParametersIsValid_Should_ReturnExpectedResultOfCarsWithoutOwners()
        {
            List<CarDto> listDto = new()
            {
                new CarDto()
                {

                    Id = 1,
                    Owner = null,
                    CarBrand = "Test Car"
                }
            };

            var expectedResultValue = _mapper.Map<IList<CarModel>>(listDto);

            _mockCarService
                .Setup(x => x.GetCarsWithoutOwners())
                .Returns(listDto);

            // Act
            var result = _controller.GetCarsWithoutOwners();

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);

            var resultValue = Assert.IsAssignableFrom<IList<CarModel>>(okObjectResult.Value);

            resultValue
                .Should()
                .BeEquivalentTo(expectedResultValue);
        }

        [Fact]
        public void Get_WhenParametersIsValid_Should_ReturnExpectedResultOfCarsByLicensePlate()
        {
            const string number = "1111 Test";
            
            var testNumber = number;

            var ownerDto = new OwnerDto()
            {
                Id = 1,
                Name = "Test Owner"
            };

            List<CarDto> listDto = new()
            {
                new CarDto()
                {
                    Id = 1,
                    Owner = ownerDto,
                    LicensePlate = "1111 Test"
                },

                new CarDto()
                {
                    Id = 2,
                    Owner = ownerDto,
                    LicensePlate = "2222 Test"
                }
            };

            var expectedResultValue = _mapper.Map<IList<CarModel>>(listDto);

            _mockCarService
                .Setup(x => x.GetCarsByLicensePlate(testNumber))
                .Returns(listDto);

            // Act
            var result = _controller.GetCarsByLicensePlate(testNumber);

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);

            var resultValue = Assert.IsAssignableFrom<IList<CarModel>>(okObjectResult.Value);

            resultValue
                .Should()
                .BeEquivalentTo(expectedResultValue);
        }

        [Fact]
        public void Get_CarsByLicensePlate_Should_ReturnNotFound()
        {
            // Arrange
            const string number = null;

            var testNumber = number;

            _mockCarService
                .Setup(x => x.GetCarsByLicensePlate(testNumber))
                .Returns(() => null);

            // Act
            var result = _controller.GetCarsByLicensePlate(testNumber);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.NotNull(result);
        }

        [Fact]
        public void Get_WhenParametersIsValid_Should_ReturnExpectedResultOfCarBrandWhithOwner()
        {
            const string carBrand = "Test Brand";

            var testBrand = carBrand;

            var ownerDto = new OwnerDto()
            {
                Id = 1,
                Name = "Test Owner"
            };

            List<CarDto> listDto = new()
            {
                new CarDto()
                {
                    Id = 1,
                    Owner = ownerDto,
                    CarBrand = "Test Brand"
                },

                new CarDto()
                {
                    Id = 2,
                    Owner = ownerDto,
                    CarBrand = "Test Brand"
                }
            };

            var expectedResultValue = _mapper.Map<IList<CarModel>>(listDto);

            _mockCarService
                .Setup(x => x.GetByCarBrandWhithOwner(testBrand))
                .Returns(listDto);

            // Act
            var result = _controller.GetCarsByCarBrand(testBrand);

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);

            var resultValue = Assert.IsAssignableFrom<IList<CarModel>>(okObjectResult.Value);

            resultValue
                .Should()
                .BeEquivalentTo(expectedResultValue);
        }

        [Fact]
        public void Get_GetCarsByCarBrand_Should_ReturnNotFound()
        {
            // Arrange
            const string carBrand = null;

            var testBrand = carBrand;

            _mockCarService
                .Setup(x => x.GetByCarBrandWhithOwner(testBrand))
                .Returns(() => null);

            // Act
            var result = _controller.GetCarsByCarBrand(testBrand);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.NotNull(result);
        }

        [Fact]
        public void Get_GetCarsByCarModel_Should_ReturnNotFound()
        {
            // Arrange
            const string model = null;

            var testModel = model;

            _mockCarService
                .Setup(x => x.GetByCarModel(testModel))
                .Returns(() => null);

            // Act
            var result = _controller.GetCarsByCarModel(testModel);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.NotNull(result);
        }

        [Fact]
        public void Get_WhenParametersIsValid_Should_ReturnExpectedResultOfCarsByCarModel()
        {
            const string model = "Test Model";

            var testModel = model;

            var ownerDto = new OwnerDto()
            {
                Id = 1,
                Name = "Test Owner"
            };

            List<CarDto> listDto = new()
            {
                new CarDto()
                {
                    Id = 1,
                    Owner = ownerDto,
                    CarBrand = "Test Model"
                },

                new CarDto()
                {
                    Id = 2,
                    Owner = ownerDto,
                    CarBrand = "Test Model"
                }
            };

            var expectedResultValue = _mapper.Map<IList<CarModel>>(listDto);

            _mockCarService
                .Setup(x => x.GetByCarModel(testModel))
                .Returns(listDto);

            // Act
            var result = _controller.GetCarsByCarModel(testModel);

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);

            var resultValue = Assert.IsAssignableFrom<IList<CarModel>>(okObjectResult.Value);

            resultValue
                .Should()
                .BeEquivalentTo(expectedResultValue);
        }

        [Fact]
        public void Get_WhenParametersIsValid_Should_ReturnExpectedResultOfCarsWhithOwnersById()
        {
            const int id = 1;

            var item = new CarDto()
            {
                    Id = 1,
                    CarBrand = "Test Car",
                    OwnerId = id
            };

            var expectedResultValue = _mapper.Map<CarModel>(item);

            _mockCarService
                .Setup(x => x.GetCarsWhithOwners(id))
                .Returns(item);

            // Act
            var result = _controller.GetCarsWhithOwnersById(id);

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);

            var resultValue = Assert.IsAssignableFrom<CarModel>(okObjectResult.Value);

            resultValue
                .Should()
                .BeEquivalentTo(expectedResultValue);
        }

        [Fact]
        public void Get_CarsWhithOwners_Should_ReturnNotFound()
        {
            const int id = 1;

            // Arrange

            _mockCarService
                .Setup(x => x.GetCarsWhithOwners(id))
                .Returns(() => null);

            // Act
            var result = _controller.GetCarsWhithOwnersById(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.NotNull(result);
        }
    }
}
