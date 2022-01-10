using Astoneti.Microservice.AutoService.Business.Contracts;
using Astoneti.Microservice.AutoService.Business.Models;
using Astoneti.Microservice.AutoService.Data.Contracts;
using Astoneti.Microservice.AutoService.Data.Entities;
using AutoMapper;
using System.Collections.Generic;

namespace Astoneti.Microservice.AutoService.Business
{
    public class OwnerService : IOwnerService
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;

        public OwnerService(IOwnerRepository ownerRepository, IMapper mapper)
        {
            _ownerRepository = ownerRepository;
            _mapper = mapper;
        }

        public List<OwnerDto> GetList()
        {
            var items = _ownerRepository.GetList();

            return _mapper.Map<List<OwnerDto>>(
                items
            );
        }

        public List<OwnerDto> GetCarsList()
        {
            var items = _ownerRepository.GetCarsList();

            return _mapper.Map<List<OwnerDto>>(
                items
            );
        }

        public List<OwnerDto> GetOwnerWhereMoreThenOneCar()
        {
            var items = _ownerRepository.GetOwnerWhereMoreThenOneCar();

            return _mapper.Map<List<OwnerDto>>(
                items
            );
        }

        public OwnerDto Get(int id)
        {
            var item = _ownerRepository.Get(id);

            return _mapper.Map<OwnerDto>(
                item
            );
        }

        public OwnerDto Add(IOwnerAddDto item)
        {
            var entity = _mapper.Map<OwnerEntity>(item);

            _ownerRepository.Insert(entity);

            return _mapper.Map<OwnerDto>(entity);
        }

        public OwnerDto Edit(IOwnerEditDto item)
        {
            var entity = _ownerRepository.Get(item.Id);

            if (entity == null)
            {
                return null;
            }

            _mapper.Map(item, entity);

            _ownerRepository.Update(entity);

            return _mapper.Map<OwnerDto>(entity);
        }

        public bool Delete(int id)
        {
            return _ownerRepository.Delete(id);
        }
    }
}
