using Astoneti.Microservice.AutoService.Data.Entities;
using Astoneti.Microservice.AutoService.Models.Car;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Astoneti.Microservice.AutoService.IntegrationTests.Controllers
{
    public class CarControllerTests : IDisposable
    {
        private readonly TestCustomWebApplicationFactory _factory;

        public CarControllerTests(ITestOutputHelper output)
        {
            _factory = new TestCustomWebApplicationFactory
            {
                Output = output
            };

            Seed();
        }
        public void Dispose()
        {
            _factory.Dispose();
        }

        private void Seed()
        {
            _factory.DbContext.Set<CarEntity>().AddRange(
                new CarEntity
                {
                    Id = 1,
                    CarBrand = "BMW",
                    Model = "X6",
                    LicensePlate = "0001 MI-7",
                    OwnerId = 1
                },
                new CarEntity
                {
                    Id = 2,
                    CarBrand = "Ford",
                    Model = "Mustang",
                    LicensePlate = "0002 MI-7",
                    OwnerId = 2
                },

                new CarEntity
                {
                    Id = 3,
                    CarBrand = "Tesla",
                    Model = "Model-S",
                    LicensePlate = "0003 MI-7",
                    OwnerId = 3
                }
            );

            _factory.DbContext.SaveChanges();
        }

        [Fact]
        public async Task GetListAsync_ReturnsOkWithList()
        {
            // Arrange
            var expectedlist = _factory.DbContext.Set<CarEntity>().ToList();

            var client = _factory.CreateClient();

            // Act
            var result = await client.GetAsync(new Uri("/cars", UriKind.Relative));

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            var resultValue = await result.Content.ReadFromJsonAsync<IList<CarModel>>();

            Assert.Equal(3, resultValue.Count);
            for (var i = 0; i < 3; i++)
            {
                Assert.Equal(expectedlist[i].Id, resultValue[i].Id);
                Assert.Equal(expectedlist[i].CarBrand, resultValue[i].CarBrand);
                Assert.Equal(expectedlist[i].Model, resultValue[i].Model);
                Assert.Equal(expectedlist[i].LicensePlate, resultValue[i].LicensePlate);
                Assert.Equal(expectedlist[i].OwnerId, resultValue[i].OwnerId);
            }
            Assert.Equal("application/json; charset=utf-8", result.Content.Headers.ContentType?.ToString());
        }

        [Fact]
        public async Task GetAsync_ById_ReturnsOkWithItem()
        {
            // Arrange
            const int id = 1;
            var expectedResult = _factory.DbContext.Set<CarEntity>().First(x => x.Id == id);

            var client = _factory.CreateClient();

            // Act
            var result = await client.GetAsync(new Uri("/cars/1", UriKind.Relative));

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("application/json; charset=utf-8", result.Content.Headers.ContentType?.ToString());

            var resultValue = await result.Content.ReadFromJsonAsync<CarModel>();
            Assert.Equal(expectedResult.Id, resultValue.Id);
        }

        [Fact]
        public async Task GetAsync_ById_ReturnsNotFound()
        {
            // Arrange
            const int id = 0;

            var client = _factory.CreateClient();

            // Act
            var result = await client.GetAsync(new Uri($"/cars/{id}", UriKind.Relative));

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task PostAsync_ReturnsCreated()
        {
            // Arrange
            var postModel = new CarPostModel
            {
                CarBrand = "Test CarBrand",
                Model = "Test Model"
            };

            var client = _factory.CreateClient();

            // Act
            var result = await client
                .PostAsJsonAsync(
                    "/cars",
                    postModel
                );

            // Assert
            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
            Assert.Equal("application/json; charset=utf-8", result.Content.Headers.ContentType?.ToString());

            var resultValue = await result.Content.ReadFromJsonAsync<CarModel>();
            Assert.NotNull(resultValue);
            Assert.Equal(postModel.CarBrand, resultValue.CarBrand);
            Assert.Equal(postModel.Model, resultValue.Model);
        }

        [Fact]
        public async Task PutAsync_ReturnsOk()
        {
            // Arrange
            var putModel = new CarPutModel
            {
                Id = 1,
                CarBrand = "New Car",
                Model = "New Model",
                LicensePlate = "New Plate",
            };

            var client = _factory.CreateClient();

            // Act
            var result = await client.PutAsJsonAsync(
                $"/cars/{putModel.Id}",
                putModel
            );

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(putModel);
            Assert.Equal("application/json; charset=utf-8", result.Content.Headers.ContentType?.ToString());
        }

        [Fact]
        public async Task PutAsync_ReturnsBadRequest()
        {
            // Arrange
            var putModel = new CarPutModel
            {
                Id = 1,
                CarBrand = "New Car",
                Model = "New Model",
                LicensePlate = "New Plate"
            };

            var client = _factory.CreateClient();

            // Act
            var result = await client.PutAsJsonAsync(
                "/cars/0",
                putModel
            );

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task PutAsync_ReturnsNotFound()
        {
            // Arrange
            const int id = 0;
            var putModel = new CarPutModel
            {
                Id = id,
                CarBrand = "New Car",
                Model = "New Model",
                LicensePlate = "New Plate"
            };

            var client = _factory.CreateClient();

            // Act
            var result = await client.PutAsJsonAsync(
                "/cars/0",
                putModel
            );

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsOk()
        {
            // Arrange
            const int id = 1;

            var client = _factory.CreateClient();

            // Act
            var result = await client.DeleteAsync(new Uri($"/cars/{id}", UriKind.Relative));

            var notFoundResult = await client.GetAsync(new Uri($"/cars/{id}", UriKind.Relative));

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.DoesNotContain(_factory.DbContext.Set<CarEntity>(), x => x.Id == id);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsNotFound()
        {
            // Arrange
            const int id = 0;

            var client = _factory.CreateClient();

            // Act
            var result = await client.DeleteAsync(new Uri($"/cars/{id}", UriKind.Relative));

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }
    }

    //[Fact]
    //public async Task GetAsync_ById_ReturnsOkWithItem()
    //{
    //    // Arrange
    //    var expectedItem = new CarModel
    //    {
    //        Id = 1,
    //        CarBrand = "BMW",
    //        Model = "X6",
    //        LicensePlate = "0001 MI-7",
    //        OwnerId = 1
    //    };

    //    var client = _factory.CreateClient();

    //    // Act
    //    var response = await client.GetAsync("/cars/1");

    //    // Assert
    //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    //    Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

    //    var stringResponse = await response.Content.ReadAsStringAsync();

    //    var actualItem = JsonConvert.DeserializeObject<CarModel>(stringResponse);

    //    actualItem.Should()
    //        .BeEquivalentTo(expectedItem);
    //}

    //[Fact]
    //public async Task GetAsync_ById_ReturnsNotFound()
    //{
    //    // Arrange
    //    const int id = 0;

    //    var client = _factory.CreateClient();

    //    // Act
    //    var actualResult = await client.GetAsync($"/cars/{id}");

    //    // Assert
    //    Assert.Equal(HttpStatusCode.NotFound, actualResult.StatusCode);
    //}

    //[Fact]
    //public async Task Post_Sould_CreateNewItem()
    //{
    //    // Arrange
    //    var expectedItem = new CarModel
    //    {
    //        Id = 4,
    //        CarBrand = "Test CarBrand",
    //        Model = "Test Model",
    //        LicensePlate = null,
    //        OwnerId = null,
    //        Owner = null
    //    };

    //    var client = _factory.CreateClient();

    //    // Act
    //    var response = await client.PostAsync(
    //        "/cars",
    //        new StringContent(
    //            JsonConvert.SerializeObject(
    //                new CarPostModel
    //                {
    //                    CarBrand = "Test CarBrand",
    //                    Model = "Test Model"
    //                }
    //            ),
    //            Encoding.UTF8,
    //            "application/json"
    //        )
    //    );

    //    // Assert
    //    Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    //    Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

    //    var stringResponse = await response.Content.ReadAsStringAsync();

    //    var actualItem = JsonConvert.DeserializeObject<CarModel>(stringResponse);

    //    actualItem.Should()
    //        .BeEquivalentTo(expectedItem);
    //}

    //[Fact]
    //public async Task PutAsync_Should_Update_Item()
    //{
    //    // Arrange
    //    var client = _factory.CreateClient();

    //    //Act
    //    var response = await client.PostAsync("/cars"
    //              , new StringContent(
    //                 JsonConvert.SerializeObject(new CarPostModel()
    //                 {
    //                     Model = "CX-7",
    //                     CarBrand = "Mazda"
    //                 }),
    //             Encoding.UTF8,
    //             "application/json"));

    //    var stringResponse = await response.Content.ReadAsStringAsync();

    //    var item = JsonConvert.DeserializeObject<CarModel>(stringResponse);

    //    var carBrand = "Test";

    //    var editCar = new CarPutModel
    //    {
    //        CarBrand = carBrand,
    //        Model = "Test",
    //        LicensePlate = "Test",
    //        Id = item.Id,
    //    };
    //    var putCont = JsonConvert.SerializeObject(editCar);

    //    var responsePut = await client.PutAsync($"/cars/{item.Id}", new StringContent(putCont, Encoding.UTF8, "application/json"));

    //    var stringResponsePut = await responsePut.Content.ReadAsStringAsync();

    //    var editedItem = JsonConvert.DeserializeObject<CarModel>(stringResponsePut);

    //    //Assert
    //    response.EnsureSuccessStatusCode();
    //    Assert.True(response.IsSuccessStatusCode);
    //    editedItem.CarBrand.Should().Be(carBrand);
    //}

    //[Fact]
    //public async Task DeleteAsync_ReturnsOk()
    //{
    //    const int id = 2;
    //    // Arrange
    //    var client = _factory.CreateClient();

    //    // Act
    //    var actualResult = await client.DeleteAsync($"/cars/{id}");

    //    var expectedItem = await client.GetAsync($"/cars/{id}");

    //    // Assert
    //    Assert.Equal(HttpStatusCode.OK, actualResult.StatusCode);
    //    Assert.Equal(HttpStatusCode.NotFound, expectedItem.StatusCode);
    //    Assert.True(actualResult.IsSuccessStatusCode);
    //}

    //[Fact]
    //public async Task DeleteAsync_ReturnsNotFound()
    //{
    //    // Arrange
    //    const int id = 0;

    //    var client = _factory.CreateClient();

    //    // Act
    //    var actualResult = await client.DeleteAsync($"/cars/{id}");

    //    // Assert
    //    Assert.Equal(HttpStatusCode.NotFound, actualResult.StatusCode);
    //}

}
