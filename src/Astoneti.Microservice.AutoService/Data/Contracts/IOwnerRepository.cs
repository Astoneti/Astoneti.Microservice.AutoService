using Astoneti.Microservice.AutoService.Data.Entities;
using System.Collections.Generic;

namespace Astoneti.Microservice.AutoService.Data.Contracts
{
    public interface IOwnerRepository
    {
        List<OwnerEntity> GetList();

        List<OwnerEntity> GetCarsList();

        List<OwnerEntity> GetOwnerWhereMoreThenOneCar();

        OwnerEntity Get(int id);

        OwnerEntity Insert(OwnerEntity entity);

        OwnerEntity Update(OwnerEntity entity);

        bool Delete(int id);
    }
}
