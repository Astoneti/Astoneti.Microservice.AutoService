using Astoneti.Microservice.AutoService.Business.Contracts;

namespace Astoneti.Microservice.AutoService.Tests.Fakes.Business
{
    public class FakeOwnerAddDto : IOwnerAddDto
    {
        public string Name { get; set; }
    }
}
