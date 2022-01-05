using Astoneti.Microservice.AutoService.Business;
using Astoneti.Microservice.AutoService.Business.Contracts;
using Astoneti.Microservice.AutoService.Business.Models;
using Astoneti.Microservice.AutoService.Data.Contracts;
using Astoneti.Microservice.AutoService.Data.Entities;
using AutoMapper;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Astoneti.Microservice.AutoService.Tests.Business
{
    public class CarServiceTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ICarRepository> _mockCarRepository;
        private readonly CarService _carService;

        public CarServiceTests()
        {
            _mockCarRepository = new Mock<ICarRepository>();

            _mapper = new MapperConfiguration(
                    cfg => cfg.AddMaps(
                        typeof(Startup).Assembly
                    )
                )
                .CreateMapper();

            _carService = new CarService(
               _mockCarRepository.Object,
               _mapper
           );
        }

        [Fact]
        public void GetList_WhenParametersIsValid_Should_ReturnExpectedResult()
        {
            // Arrange
            List<CarEntity> entities = new()
            {
                new CarEntity() { Id = 1, CarBrand = "Mazda", Model = "CX-7", LicensePlate = "5757 IX-7" },
            };

            _mockCarRepository
                .Setup(x => x.GetList())
                .Returns(entities);

            var expectedResultValue = _mapper.Map<IList<CarDto>>(entities);

            // Act
            var result = _carService.GetList();

            // Assert
            Assert.IsType<List<CarDto>>(result);
            Assert.NotNull(result);
            Assert.Equal(expectedResultValue.Count, result.Count);
        }

        [Fact]
        public void Get_Should_ReturnItemById()
        {
            // Arrange
            const int id = 1;

            CarEntity entity = new()
            {
                Id = id,
                CarBrand = "Test Car",
                Model = "TestModel",
            };

            _mockCarRepository
                .Setup(x => x.Get(id))
                .Returns(entity);

            var expectedResultValue = _mapper.Map<CarDto>(entity);

            // Act
            var result = _carService.Get(id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<CarDto>(result);
            Assert.Equal(expectedResultValue.Id, result.Id);
            Assert.Equal(1, 1);
        }

        [Fact]
        public void Add_Should_CreateNewItemAndInsertInDb()
        {
            // Arrange
            CarEntity entity = new()
            {
                CarBrand = "Test Car",
                Model = "Test Car"
            };

            ICarAddDto item = _mapper.Map<ICarAddDto>(entity);

            CarEntity expectedItem = new();

            _mockCarRepository
                .Setup(x => x.Insert(It.IsAny<CarEntity>()))
                .Callback((CarEntity newEntity) =>
                {
                    expectedItem = newEntity;
                });

            // Act
            _carService.Add(item);

            // Assert
            expectedItem
                .Should().BeEquivalentTo(item);

            _mockCarRepository
                .Verify(x => x.Insert(It.IsAny<CarEntity>()), Times.Once);
            Assert.NotNull(expectedItem);
        }

        [Fact]
        public void Edit_WhenItemIsNull_Should_ReturnNull()
        {
            //Arrange
           CarDto dto = new()
           {
               Id = 1,
               CarBrand = "Test Car",
               Model = "Test Car"
           };

            CarEntity expectedItem = new()
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
            var result = _carService.Edit(editDto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Edit_Should_FindItemByIdAndReturnsUpdatedItem()
        {

            // Arrange
            CarDto dto = new()
            {
                Id = 1,
                CarBrand = "Test Car",
                Model = "Test Car"
            };

            CarEntity expectedItem = new()
            {
                Id = 1,
                CarBrand = "Test New Car",
                Model = "Test New Car"
            };

            var item = _mapper.Map<ICarEditDto>(expectedItem);

            _mockCarRepository
                .Setup(x => x.Get(dto.Id))
                .Returns(expectedItem);

            _mockCarRepository
                 .Setup(x => x.Update(expectedItem))
                 .Returns(expectedItem);

            // Act
            var result = _carService.Edit(item);

            //Assert
            expectedItem
                .Should().BeEquivalentTo(result);
            Assert.NotNull(result);
        }

        [Fact]
        public void Delete_Should_FindItemByIdAndRemove()
        {
            // Arrange
            const int id = 1;

            bool operationResult = true;

            _mockCarRepository
                .Setup(x => x.Delete(id))
                .Returns(operationResult);

            // Act
            bool result = _carService.Delete(id);

            //Assert
            result
                .Should()
                .Be(operationResult);
        }
    }
}
