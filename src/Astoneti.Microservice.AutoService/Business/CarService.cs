using Astoneti.Microservice.AutoService.Business.Contracts;
using Astoneti.Microservice.AutoService.Business.Models;
using Astoneti.Microservice.AutoService.Data.Contracts;
using Astoneti.Microservice.AutoService.Data.Entities;
using AutoMapper;
using System.Collections.Generic;

namespace Astoneti.Microservice.AutoService.Business
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;
        private readonly IMapper _mapper;

        public CarService(ICarRepository carRepository, IMapper mapper)
        {
            _carRepository = carRepository;
            _mapper = mapper;
        }

        public List<CarDto> GetList()
        {
            var items = _carRepository.GetList();

            return _mapper.Map<List<CarDto>>(
                items
            );
        }

        public List<CarDto> GetOwnersList()
        {
            var items = _carRepository.GetOwnersList();

            return _mapper.Map<List<CarDto>>(
                items
            );
        }

        public List<CarDto> GetCarsWithoutOwners()
        {
            var items = _carRepository.GetCarsWithoutOwners();

            return _mapper.Map<List<CarDto>>(
                items
            );
        }

        public List<CarDto> GetCarsByLicensePlate(string number)
        {
            var items = _carRepository.GetCarsByLicensePlate(number);

            return _mapper.Map<List<CarDto>>(
                items
            );
        }

        public List<CarDto> GetByCarBrandWhithOwner(string brand)
        {
            var items = _carRepository.GetByCarBrandWhithOwner(brand);

            return _mapper.Map<List<CarDto>>(
                items
            );
        }

        public List<CarDto> GetByCarModel(string model)
        {
            var items = _carRepository.GetByCarModel(model);

            return _mapper.Map<List<CarDto>>(
                items
            );
        }

        public CarDto Get(int id)
        {
            var item = _carRepository.Get(id);

            return _mapper.Map<CarDto>(
                item
            );
        }

        public CarDto GetCarsWhithOwners(int id)
        {
            var item = _carRepository.GetCarsWhithOwners(id);

            return _mapper.Map<CarDto>(
                item
            );
        }

        public CarDto Add(ICarAddDto item)
        {
            var entity = _mapper.Map<CarEntity>(item);

            entity = _carRepository.Insert(entity);

            return _mapper.Map<CarDto>(entity);
        }

        public CarDto Edit(ICarEditDto item)
        {
            var entity = _carRepository.Get(item.Id);

            if (entity == null)
            {
                return null;
            }

            _mapper.Map(item, entity);

            entity = _carRepository.Update(entity);

            return _mapper.Map<CarDto>(entity);
        }

        public bool Delete(int id)
        {
            return _carRepository.Delete(id);
        }
    }
}
