using System.Collections.Generic;

namespace Astoneti.Microservice.AutoService.Data.Entities
{
    public class OwnerEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<CarEntity> Cars { get; set; }
    }
}
