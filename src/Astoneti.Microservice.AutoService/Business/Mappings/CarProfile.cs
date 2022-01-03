using Astoneti.Microservice.AutoService.Business.Contracts;
using Astoneti.Microservice.AutoService.Business.Models;
using Astoneti.Microservice.AutoService.Data.Entities;
using AutoMapper;

namespace Astoneti.Microservice.AutoService.Business.Mappings
{
    public class CarProfile : Profile
    {
        public CarProfile()
        {
            CreateMap<CarEntity, CarDto>().ReverseMap();

            CreateMap<CarDto, CarEntity>().ReverseMap();

            CreateMap<ICarAddDto, CarEntity>().ReverseMap();

            CreateMap<ICarEditDto, CarEntity>().ReverseMap();
        }
    }
}
