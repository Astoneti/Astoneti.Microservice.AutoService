using Astoneti.Microservice.AutoService.Business.Models;
using Astoneti.Microservice.AutoService.Models.Car;
using Astoneti.Microservice.AutoService.Models.Owner;
using AutoMapper;

namespace Astoneti.Microservice.AutoService.Mappings
{
    public class OwnerProfile : Profile
    {
        public OwnerProfile() 
        {
            CreateMap<OwnerModel, OwnerDto>().ReverseMap();

            CreateMap<CarOwnerModel, OwnerDto>().ReverseMap();
        }
    }
}
