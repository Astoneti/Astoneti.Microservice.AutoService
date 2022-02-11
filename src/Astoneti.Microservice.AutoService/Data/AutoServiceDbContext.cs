using Astoneti.Microservice.AutoService.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Astoneti.Microservice.AutoService.Data
{
    public class AutoServiceDbContext : DbContext
    {
        public AutoServiceDbContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        {

        }

        public DbSet<CarEntity> Cars { get; set; }

        public DbSet<OwnerEntity> Owners { get; set; }
    }
}
