using Astoneti.Microservice.AutoService.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Astoneti.Microservice.AutoService.Data
{
    public class AutoServiceDbContext : DbContext
    {
        public AutoServiceDbContext(DbContextOptions<AutoServiceDbContext> options)
            : base(options)
        {

        }

        public DbSet<CarEntity> Cars { get; set; }

        public DbSet<OwnerEntity> Owners { get; set; }
    }
}
