using Astoneti.Microservice.AutoService.Business;
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
    public class OwnerServiceTests
    {
        private readonly Mock<IOwnerRepository> _mockOwnerRepository;
        private readonly IMapper _mapper;

        private readonly OwnerService _service;

        public OwnerServiceTests()
        {
            _mockOwnerRepository = new Mock<IOwnerRepository>();

            _mapper = new MapperConfiguration(
                    cfg => cfg.AddMaps(
                        typeof(Startup).Assembly
                    )
                )
                .CreateMapper();

            _service = new OwnerService(
               _mockOwnerRepository.Object,
               _mapper
           );
        }

        [Fact]
        public void GetList_Should_ReturnExpectedResult()
        {
            // Arrange
            var entities = new List<OwnerEntity>()
            {
                new OwnerEntity()
                {
                    Id = 1,
                    Name = "Test Owner"
                },
            };

            _mockOwnerRepository
                .Setup(x => x.GetList())
                .Returns(entities);

            var expectedResult = _mapper.Map<List<OwnerDto>>(entities);

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

            var entity = new OwnerEntity()
            {
                Id = id,
                Name = "Test Owner",
            };

            _mockOwnerRepository
                .Setup(x => x.Get(id))
                .Returns(entity);

            var expectedResult = _mapper.Map<OwnerDto>(entity);

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
            var item = new FakeOwnerAddDto()
            {
                Name = "Test Owner"
            };

            var entity = _mapper.Map<OwnerEntity>(item);

            // Act
            _mockOwnerRepository
               .Setup(x => x.Insert(entity))
               .Returns(entity);

            var expectedResult = _mapper.Map<OwnerDto>(entity);

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
            const int id = 1;

            var item = new FakeOwnerEditDto()
            {
                Id = id,
                Name = "Test Owner",
            };

            var entity = new OwnerEntity()
            {
                Id = id,
                Name = "Test New Owner"
            };

            _mockOwnerRepository
                .Setup(x => x.Get(id))
                .Returns(() => null);

            _mapper.Map<OwnerDto>(entity);

            // Act
            var result = _service.Edit(item);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Edit_Should_FindItemByIdAndReturnsUpdatedItem()
        {
            // Arrange
            const int id = 1;

            var item = new FakeOwnerEditDto()
            {
                Id = id,
                Name = "Test Owner"
            };

            var entity = _mapper.Map<OwnerEntity>(item);

            _mockOwnerRepository
                .Setup(x => x.Get(id))
                .Returns(entity);

            _mapper.Map(item, entity);

            _mockOwnerRepository
                .Setup(x => x.Update(entity))
                .Returns(entity);

            var expectedResult = _mapper.Map<OwnerDto>(entity);

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

            _mockOwnerRepository
                .Setup(x => x.Delete(id))
                .Returns(true);

            // Act
            var result = _service.Delete(id);

            // Assert
            Assert.True(result);
        }
    }
}

