using Astoneti.Microservice.AutoService.Business.Contracts;

namespace Astoneti.Microservice.AutoService.Models.Owner
{
    public class OwnerPostModel : IOwnerAddDto
    {
        public string Name { get; set; }
    }
}
