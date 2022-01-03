using System.Collections.Generic;

namespace Astoneti.Microservice.AutoService.Business.Models
{
    public class OwnerDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<CarDto> Cars { get; set; }
    }
}
