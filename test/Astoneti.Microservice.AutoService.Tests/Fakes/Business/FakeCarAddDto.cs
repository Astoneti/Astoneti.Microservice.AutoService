using Astoneti.Microservice.AutoService.Business.Contracts;

namespace Astoneti.Microservice.AutoService.Tests.Fakes.Business
{
    public class FakeCarAddDto : ICarAddDto
    {
        public string CarBrand { get; set; }

        public string Model { get; set; }
    }
}
