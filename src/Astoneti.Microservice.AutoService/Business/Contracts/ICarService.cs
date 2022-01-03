using Astoneti.Microservice.AutoService.Business.Models;
using System.Collections.Generic;

namespace Astoneti.Microservice.AutoService.Business.Contracts
{
    public interface ICarService
    {
        List<CarDto> GetList();

        List<CarDto> GetOwnersList();

        List<CarDto> GetCarsWithoutOwners();

        List<CarDto> GetCarsByLicensePlate(string number);

        List<CarDto> GetByCarBrandWhithOwner(string brand);

        List<CarDto> GetByCarModel(string  model);

        CarDto Get(int id);

        CarDto GetCarsWhithOwners(int id);

        CarDto Add(ICarAddDto item);

        CarDto Edit(ICarEditDto item);

        bool Delete(int id);
    }
}
