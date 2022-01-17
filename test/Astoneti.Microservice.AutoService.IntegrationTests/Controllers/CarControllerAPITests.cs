using Astoneti.Microservice.AutoService.Models.Car;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Astoneti.Microservice.AutoService.IntegrationTests.Controllers
{
    public class CarControllerAPITests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public CarControllerAPITests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetList_Should_Return_Result_StatusCodeOK()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var result = await client.GetAsync("/cars");

            // Assert
            result.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("application/json; charset=utf-8", result.Content.Headers.ContentType?.ToString());
        }

        [Fact]
        public async Task GetById_Should_Return_Result_NotFound()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var result = await client.GetAsync("cars/100");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task GetById_Should_Return_Result_ItemById()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var httpResponse = await client.GetAsync("/cars/2");

            // Assert
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();

            var car = JsonConvert.DeserializeObject<CarModel>(stringResponse);
            Assert.Equal(2, car.Id);
            Assert.Equal("Mazda", car.CarBrand);
        }

        [Fact]
        public async Task Test_Post()
        {
            // Arrange
            var client = _factory.CreateClient();
            {
                var result = await client.PostAsync("/cars"
                     , new StringContent(
                        JsonConvert.SerializeObject(new CarPostModel()
                        {
                            Model = "CX-7",
                            CarBrand = "Mazda"
                        }),
                    Encoding.UTF8,
                    "application/json"));
                // Act
                result.EnsureSuccessStatusCode();

                // Assert
                Assert.Equal(HttpStatusCode.Created, result.StatusCode);
            }
        }

        [Fact]
        public async Task Test_Put()
        {
            // Arrange
            var client = _factory.CreateClient();
            {
                var result = await client.PutAsync("/cars/2"
                     , new StringContent(
                        JsonConvert.SerializeObject(new CarPutModel()
                        {
                            Id = 2,
                            Model = "Test",
                            CarBrand = "Mazda",
                            LicensePlate = "5757 IX-7"
                        }),
                    Encoding.UTF8,
                    "application/json"));

                // Act
                result.EnsureSuccessStatusCode();

                // Assert
                Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            }
        }

        [Fact]
        public async Task Delete_Should_Return_NotFound_Result()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var result = await client.DeleteAsync("/cars/100");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }
    }
}
