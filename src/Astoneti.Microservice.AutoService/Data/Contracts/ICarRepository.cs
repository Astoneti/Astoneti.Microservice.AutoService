using Astoneti.Microservice.AutoService.Data.Entities;
using System.Collections.Generic;

namespace Astoneti.Microservice.AutoService.Data.Contracts
{
    public interface ICarRepository
    {
        List<CarEntity> GetList();

        List<CarEntity> GetOwnersList();

        List<CarEntity> GetCarsWithoutOwners();

        List<CarEntity> GetCarsListWhithOwners();

        List<CarEntity> GetCarsByLicensePlate(string entity);

        List<CarEntity> GetByCarBrandWhithOwner(string entity);

        List<CarEntity> GetByCarModel(string entity);

        CarEntity Get(int id);

        CarEntity GetCarsWhithOwners(int id);

        CarEntity Insert(CarEntity entity);

        CarEntity Update(CarEntity entity);

        bool Delete(int id);
    }
}
