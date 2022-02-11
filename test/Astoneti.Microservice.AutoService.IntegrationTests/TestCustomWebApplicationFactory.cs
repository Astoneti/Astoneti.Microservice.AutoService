using Astoneti.Microservice.AutoService.Data;
using Astoneti.Microservice.AutoService.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;

namespace Astoneti.Microservice.AutoService.IntegrationTests
{
    public class TestCustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
        private readonly DbContextOptions _dbContextOptions;

        public TestCustomWebApplicationFactory()
        {
            _dbContextOptions = new DbContextOptionsBuilder()
                .UseInMemoryDatabase("InMemoryDbForTesting")
                .Options;

            DbContext = new AutoServiceDbContext(_dbContextOptions);
        }

        public ITestOutputHelper Output { get; set; }
        public AutoServiceDbContext DbContext { get; }

        protected override IHostBuilder CreateHostBuilder()
        {
            var builder = base.CreateHostBuilder();

            builder.ConfigureServices(
                services =>
                {
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AutoServiceDbContext>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    services.AddDbContext<AutoServiceDbContext>(
                        options => options.UseInMemoryDatabase("InMemoryDbForTesting")
                    );
                }
            );

            return builder;
        }

        //    private static void InitializeDbForTests(AutoServiceDbContext db)
        //    {
        //        var listEntities = new List<CarEntity>()
        //            {
        //                new CarEntity
        //                {
        //                    Id = 1,
        //                    CarBrand = "BMW",
        //                    Model = "X6",
        //                    LicensePlate = "0001 MI-7",
        //                    OwnerId = 1
        //                },

        //                new CarEntity
        //                {
        //                    Id = 2,
        //                    CarBrand = "Ford",
        //                    Model = "Mustang",
        //                    LicensePlate = "0002 MI-7",
        //                    OwnerId = 2
        //                },

        //                new CarEntity
        //                {
        //                    Id = 3,
        //                    CarBrand = "Tesla",
        //                    Model = "Model-S",
        //                    LicensePlate = "0003 MI-7",
        //                    OwnerId = 3
        //                }
        //            };

        //        db.Cars.AddRange(listEntities);

        //        db.SaveChanges();
        //    }
        //}

        //public class TestCustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
        //    where TStartup : class
        //{
        //    protected override void ConfigureWebHost(IWebHostBuilder builder)
        //    {
        //        builder.ConfigureServices(services =>
        //        {
        //            var descriptor = services.SingleOrDefault(
        //                d => d.ServiceType ==
        //                    typeof(DbContextOptions<AutoServiceDbContext>));

        //            services.Remove(descriptor);

        //            services.AddDbContext<AutoServiceDbContext>(options =>
        //            {
        //                options.UseInMemoryDatabase("InMemoryDbForTesting");
        //            });

        //            var sp = services.BuildServiceProvider();

        //            using (var scope = sp.CreateScope())
        //            {
        //                var scopedServices = scope.ServiceProvider;

        //                var db = scopedServices.GetRequiredService<AutoServiceDbContext>();

        //                var logger = scopedServices
        //                    .GetRequiredService<ILogger<TestCustomWebApplicationFactory<TStartup>>>();

        //                db.Database.EnsureCreated();

        //                try
        //                {
        //                    InitializeDbForTests(db);
        //                }
        //                catch (Exception ex)
        //                {
        //                    logger.LogError(ex, "An error occurred seeding the " +
        //                        "database with test messages. Error: {Message}", ex.Message);
        //                }
        //            }
        //        });
        //    }
        //    private static void InitializeDbForTests(AutoServiceDbContext db)
        //    {
        //        var listEntities = new List<CarEntity>()
        //            {
        //                new CarEntity
        //                {
        //                    Id = 1,
        //                    CarBrand = "BMW",
        //                    Model = "X6",
        //                    LicensePlate = "0001 MI-7",
        //                    OwnerId = 1
        //                },

        //                new CarEntity
        //                {
        //                    Id = 2,
        //                    CarBrand = "Ford",
        //                    Model = "Mustang",
        //                    LicensePlate = "0002 MI-7",
        //                    OwnerId = 2
        //                },

        //                new CarEntity
        //                {
        //                    Id = 3,
        //                    CarBrand = "Tesla",
        //                    Model = "Model-S",
        //                    LicensePlate = "0003 MI-7",
        //                    OwnerId = 3
        //                }
        //            };

        //        db.Cars.AddRange(listEntities);

        //        db.SaveChanges();
        //    }
    }
}

