using Astoneti.Microservice.AutoService.Business.Models;
using System.Collections.Generic;

namespace Astoneti.Microservice.AutoService.Business.Contracts
{
    public interface IOwnerService
    {
        List<OwnerDto> GetList();

        List<OwnerDto> GetCarsList();

        List<OwnerDto> GetOwnerWhereMoreThenOneCar();

        OwnerDto Get(int id);

        OwnerDto Add(IOwnerAddDto item);

        OwnerDto Edit(IOwnerEditDto item);

        bool Delete(int id);
    }
}
