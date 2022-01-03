using Astoneti.Microservice.AutoService.Data.Contracts;
using Astoneti.Microservice.AutoService.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Astoneti.Microservice.AutoService.Data
{
    public class OwnerRepository : IOwnerRepository
    {
        public OwnerRepository(AutoServiceDbContext dbContext)
        {
            DbContext = dbContext;
        }

        protected DbContext DbContext { get; }

        public List<OwnerEntity> GetList()
        {
            return DbContext.Set<OwnerEntity>().ToList();           
        }

        public List<OwnerEntity> GetCarsList()
        {
            return DbContext.Set<OwnerEntity>().Include(x => x.Cars).ToList();
        }

        public List<OwnerEntity> GetOwnerWhereMoreThenOneCar()
        {
            return DbContext.Set<OwnerEntity>().Include(x => x.Cars).Where(x => x.Cars.Count > 1).ToList();
        }

        public OwnerEntity Get(int id)
        {
            return DbContext.Set<OwnerEntity>().FirstOrDefault(x => x.Id == id);
        }

        public OwnerEntity Insert(OwnerEntity entity)
        {
            DbContext.Set<OwnerEntity>().Add(entity);

            DbContext.SaveChanges();

            return entity;
        }

        public OwnerEntity Update(OwnerEntity entity)
        {
            DbContext.Set<OwnerEntity>().Update(entity);

            DbContext.SaveChanges();

            return entity;
        }

        public bool Delete(int id)
        {
            var entity = DbContext.Set<OwnerEntity>().Include(c => c.Cars).FirstOrDefault(_ =>_.Id == id);

            if (entity == null)
            {
                return false;
            }

            DbContext.Set<OwnerEntity>().Remove(entity);

            var result = DbContext.SaveChanges();

            return result == 1;
        }
    }
}
