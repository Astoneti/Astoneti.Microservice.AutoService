using Astoneti.Microservice.AutoService.Business;
using Astoneti.Microservice.AutoService.Business.Contracts;
using Astoneti.Microservice.AutoService.Business.Models;
using Astoneti.Microservice.AutoService.Data.Contracts;
using Astoneti.Microservice.AutoService.Data.Entities;
using Astoneti.Microservice.AutoService.Tests.Fakes.Business;
using AutoMapper;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Astoneti.Microservice.AutoService.Tests.Business
{
    public class CarServiceTests
    {
        private readonly Mock<ICarRepository> _mockCarRepository;
        private readonly IMapper _mapper;

        private readonly CarService _service;

        public CarServiceTests()
        {
            _mockCarRepository = new Mock<ICarRepository>();

            _mapper = new MapperConfiguration(
                    cfg => cfg.AddMaps(
                        typeof(Startup).Assembly
                    )
                )
                .CreateMapper();

            _service = new CarService(
               _mockCarRepository.Object,
               _mapper
           );
        }

        [Fact]
        public void GetList_Should_ReturnExpectedResult()
        {
            // Arrange
            var entities = new List<CarEntity>()
            {
                new CarEntity() {
                    Id = 1,
                    CarBrand = "Test Car",
                    Model = "TestModel",
                    LicensePlate = "Test Plate"
                },
            };

            _mockCarRepository
                .Setup(x => x.GetList())
                .Returns(entities);

            var expectedResult = _mapper.Map<List<CarDto>>(entities);

            // Act
            var result = _service.GetList();

            // Assert
            result
                .Should()
                .BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void Get_Should_ReturnItemById()
        {
            // Arrange
            const int id = 1;

            var entity = new CarEntity()
            {
                Id = id,
                CarBrand = "Test Car",
                Model = "TestModel",
            };

            _mockCarRepository
                .Setup(x => x.Get(id))
                .Returns(entity);

            var expectedResult = _mapper.Map<CarDto>(entity);

            // Act
            var result = _service.Get(id);

            // Assert
            result
                .Should()
                .BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void Add_Should_CreateNewItem()
        {
            // Arrange
            var item = new FakeCarAddDto()
            {
                CarBrand = "Test CarBrand",
                Model = "Test Model"
            };

            var entity = _mapper.Map<CarEntity>(item);

            _mockCarRepository
                .Setup(x => x.Insert(entity))
                .Returns(entity);

            var expectedResult = _mapper.Map<CarDto>(entity);

            // Act
            var result = _service.Add(item);

            // Assert
            result
                .Should()
                .BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void Edit_WhenItemIsNull_Should_ReturnNull()
        {
            // Arrange
            var dto = new CarDto()
            {
                Id = 1,
                CarBrand = "Test Car",
                Model = "Test Car"
            };

            var expectedItem = new CarEntity()
            {
                Id = 1,
                CarBrand = "Test New Car",
                Model = "Test New Car"
            };

            _mockCarRepository
                .Setup(x => x.Get(dto.Id))
                .Returns(() => null);

            var editDto = _mapper.Map<ICarEditDto>(expectedItem);

            // Act
            var result = _service.Edit(editDto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Edit_Should_FindItemByIdAndReturnsUpdatedItem()
        {
            // Arrange
            const int id = 1;

            var item = new FakeCarEditDto()
            {
                Id = id,
                CarBrand = "Test Brand",
                Model = "Test Model",
                LicensePlate = "Test Plate",
                OwnerId = 1
            };

            var entity = _mapper.Map<CarEntity>(item);

            _mockCarRepository
               .Setup(x => x.Get(id))
               .Returns(entity);

            _mapper.Map(item, entity);

            _mockCarRepository
                .Setup(x => x.Update(entity))
                .Returns(entity);

            var expectedResult = _mapper.Map<CarDto>(entity);

            // Act
            var result = _service.Edit(item);

            // Assert
            result
                .Should()
                .BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void Delete_Should_FindItemByIdAndRemove()
        {
            // Arrange
            const int id = 1;

            _mockCarRepository
                .Setup(x => x.Delete(id))
                .Returns(true);

            // Act
            var result = _service.Delete(id);

            // Assert
            Assert.True(result);
        }
    }
}
