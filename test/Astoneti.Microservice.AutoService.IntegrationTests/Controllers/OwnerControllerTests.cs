using Astoneti.Microservice.AutoService.Data.Entities;
using Astoneti.Microservice.AutoService.Models.Owner;
using Microsoft.EntityFrameworkCore;
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
    public class OwnerControllerTests : IDisposable
    {
        private readonly TestCustomWebApplicationFactory _factory;

        public OwnerControllerTests(ITestOutputHelper output)
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
            _factory.DbContext.Set<OwnerEntity>().AddRange(
                new OwnerEntity
                {
                    Id = 1,
                    Name = "First Test Owner",
                    Cars = new List<CarEntity> { new CarEntity { Id = 1,Model = "Test Car" } }
                },
                new OwnerEntity
                {
                    Id = 2,
                    Name = "Second Test Owner",
                    Cars = new List<CarEntity> { new CarEntity { Id = 2, Model = "Test Car" } }
                }
            );

            _factory.DbContext.SaveChanges();
        }

        [Fact]
        public async Task GetListAsync_ReturnsOkWithList()
        {
            // Arrange
            var expectedlist = _factory.DbContext.Set<OwnerEntity>().ToList();

            var client = _factory.CreateClient();

            // Act
            var result = await client.GetAsync(new Uri("/owners", UriKind.Relative));

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            var resultValue = await result.Content.ReadFromJsonAsync<IList<OwnerModel>>();

            Assert.Equal(2, resultValue.Count);
            for (var i = 0; i < 2; i++)
            {
                Assert.Equal(expectedlist[i].Id, resultValue[i].Id);
                Assert.Equal(expectedlist[i].Name, resultValue[i].Name);
            }
            Assert.Equal("application/json; charset=utf-8", result.Content.Headers.ContentType?.ToString());
        }

        [Fact]
        public async Task GetAsync_ById_ReturnsOkWithItem()
        {
            // Arrange
            const int id = 1;
            var expectedResult = _factory.DbContext.Set<OwnerEntity>().First(x => x.Id == id);

            var client = _factory.CreateClient();

            // Act
            var result = await client.GetAsync(new Uri("/owners/1", UriKind.Relative));

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("application/json; charset=utf-8", result.Content.Headers.ContentType?.ToString());

            var resultValue = await result.Content.ReadFromJsonAsync<OwnerModel>();
            Assert.Equal(expectedResult.Id, resultValue.Id);
        }

        [Fact]
        public async Task GetAsync_ById_ReturnsNotFound()
        {
            // Arrange
            const int id = 0;

            var client = _factory.CreateClient();

            // Act
            var result = await client.GetAsync(new Uri($"/owners/{id}", UriKind.Relative));

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task PostAsync_ReturnsCreated()
        {
            // Arrange
            var postModel = new OwnerPostModel
            {
                Name = "New Test Owner"
            };

            var client = _factory.CreateClient();

            // Act
            var result = await client
                .PostAsJsonAsync(
                    "/owners",
                    postModel
                );

            // Assert
            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
            Assert.Equal("application/json; charset=utf-8", result.Content.Headers.ContentType?.ToString());

            var resultValue = await result.Content.ReadFromJsonAsync<OwnerModel>();
            Assert.NotNull(resultValue);
            Assert.Equal(postModel.Name, resultValue.Name);

            var entity = Assert.Single(_factory.DbContext.Set<OwnerEntity>(), x => x.Name == postModel.Name);
            Assert.NotNull(entity);
        }

        [Fact]
        public async Task PutAsync_ReturnsOk()
        {
            // Arrange
            var putModel = new OwnerPutModel
            {
                Id = 1,
                Name = "First Test Owner",
                CarId = 1,
            };

            var client = _factory.CreateClient();

            // Act
            var result = await client.PutAsJsonAsync(
                $"/owners/{putModel.Id}",
                putModel
            );

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);

            var entity = Assert.Single(_factory.DbContext.Set<OwnerEntity>().AsNoTracking(), x => x.Id == putModel.Id);
            Assert.NotNull(entity);
            Assert.Equal(putModel.Id, entity.Id);
            Assert.Equal(putModel.Name, entity.Name);
        }

        [Fact]
        public async Task PutAsync_ReturnsBadRequest()
        {
            // Arrange
            var putModel = new OwnerPutModel
            {
                Id = 1,
                Name = "New Owner",
                CarId = 1
            };

            var client = _factory.CreateClient();

            // Act
            var result = await client.PutAsJsonAsync(
                "/owners/0",
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
            var putModel = new OwnerPutModel
            {
                Id = id,
                Name = "New Test Owner"
            };

            var client = _factory.CreateClient();

            // Act
            var result = await client.PutAsJsonAsync(
                "/owners/0",
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
            var result = await client.DeleteAsync(new Uri($"/owners/{id}", UriKind.Relative));

            var notFoundResult = await client.GetAsync(new Uri($"/owners/{id}", UriKind.Relative));

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.DoesNotContain(_factory.DbContext.Set<OwnerEntity>(), x => x.Id == id);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsNotFound()
        {
            // Arrange
            const int id = 0;

            var client = _factory.CreateClient();

            // Act
            var result = await client.DeleteAsync(new Uri($"/owners/{id}", UriKind.Relative));

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }
    }
}
