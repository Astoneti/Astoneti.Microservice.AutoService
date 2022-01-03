using Astoneti.Microservice.AutoService.Business.Contracts;

namespace Astoneti.Microservice.AutoService.Models.Car
{
    public class CarPostModel : ICarAddDto
    {
        public string CarBrand { get; set; }

        public string Model { get; set; }
    }
}
