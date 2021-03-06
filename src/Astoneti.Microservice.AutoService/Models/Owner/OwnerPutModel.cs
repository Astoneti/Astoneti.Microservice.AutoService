using Astoneti.Microservice.AutoService.Business.Contracts;

namespace Astoneti.Microservice.AutoService.Models.Owner
{
    public class OwnerPutModel : IOwnerEditDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CarId { get; set; }
    }
}
