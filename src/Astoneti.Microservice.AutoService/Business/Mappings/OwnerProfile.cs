using Astoneti.Microservice.AutoService.Business.Contracts;
using Astoneti.Microservice.AutoService.Business.Models;
using Astoneti.Microservice.AutoService.Data.Entities;
using AutoMapper;

namespace Astoneti.Microservice.AutoService.Business.Mappings
{
    public class OwnerProfile : Profile
    {
        public OwnerProfile()
        {
            CreateMap<OwnerEntity, OwnerDto>().ReverseMap();

            CreateMap<OwnerDto, OwnerEntity>().ReverseMap();

            CreateMap<IOwnerAddDto, OwnerEntity>().ReverseMap();

            CreateMap<ICarEditDto, OwnerEntity>().ReverseMap();
        }
    }
}
