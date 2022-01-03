using Astoneti.Microservice.AutoService.Business.Models;
using Astoneti.Microservice.AutoService.Models.Car;
using AutoMapper;

namespace Astoneti.Microservice.AutoService.Mappings
{
    public class CarProfile : Profile
    {
        public CarProfile() 
        {
            CreateMap<CarModel, CarDto>().ReverseMap();
        }
    }
}
