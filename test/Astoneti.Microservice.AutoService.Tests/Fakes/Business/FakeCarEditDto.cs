using Astoneti.Microservice.AutoService.Business.Contracts;

namespace Astoneti.Microservice.AutoService.Tests.Fakes.Business
{
    public class FakeCarEditDto : ICarEditDto
    {
        public int Id { get; set; }

        public string CarBrand { get; set; }

        public string Model { get; set; }

        public string LicensePlate { get; set; }

        public int OwnerId { get; set; }
    }
}
