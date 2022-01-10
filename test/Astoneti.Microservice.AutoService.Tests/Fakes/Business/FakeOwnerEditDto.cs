using Astoneti.Microservice.AutoService.Business.Contracts;

namespace Astoneti.Microservice.AutoService.Tests.Fakes.Business
{
    public class FakeOwnerEditDto : IOwnerEditDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CarId { get; set; }
    }
}
