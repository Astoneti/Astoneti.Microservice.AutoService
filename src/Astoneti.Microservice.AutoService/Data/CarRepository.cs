using Astoneti.Microservice.AutoService.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Astoneti.Microservice.AutoService.Data.Contracts
{
    public class CarRepository : ICarRepository         
    {
        public CarRepository(AutoServiceDbContext dbContext)
        {
            DbContext = dbContext;
        }

        protected DbContext DbContext { get; }

        public List<CarEntity> GetList()
        {
            return DbContext.Set<CarEntity>().ToList();
        }

        public List<CarEntity> GetOwnersList()
        {
            return DbContext.Set<CarEntity>().Include(x => x.Owner).ToList();
        }

        public List<CarEntity> GetCarsWithoutOwners()
        {
            return DbContext.Set<CarEntity>().Include(x => x.Owner).Where(y => y.Owner == null).ToList();
        }

        public List<CarEntity> GetCarsByLicensePlate(string number)
        {
            return DbContext.Set<CarEntity>().Include(y => y.Owner).Where(c => c.LicensePlate == number).ToList();
        }

        public List<CarEntity> GetByCarBrandWhithOwner(string brand)
        {
            return DbContext.Set<CarEntity>().Include(y => y.Owner).Where(c => c.CarBrand == brand).ToList();
        }

        public List<CarEntity> GetByCarModel(string model)
        {
            return DbContext.Set<CarEntity>().Include(y => y.Owner).Where(c => c.Model == model).ToList();
        }

        public List<CarEntity> GetCarsListWhithOwners()
        {
            return DbContext.Set<CarEntity>().Include(y => y.Owner).ToList();
        }

        public CarEntity Get(int id)
        {
            return DbContext.Set<CarEntity>().FirstOrDefault(x => x.Id == id);
        }

        public CarEntity GetCarsWhithOwners(int id)
        {
            return DbContext.Set<CarEntity>().Include(y => y.Owner)
                .FirstOrDefault(x => x.Id == id);
        }

        public CarEntity Insert(CarEntity entity)
        {
            DbContext.Set<CarEntity>().Add(entity);

            DbContext.SaveChanges();

            return entity;
        }

        public CarEntity Update(CarEntity entity)
        {
            DbContext.Set<CarEntity>().Update(entity);

            DbContext.SaveChanges();

            return entity;
        }

        public bool Delete(int id)
        {
            var entity = DbContext.Set<CarEntity>().Find(id);

            if (entity == null)
            {
                return false;
            }

            DbContext.Set<CarEntity>().Remove(entity);

            var result = DbContext.SaveChanges();

            return result == 1;
        }
    }
}
