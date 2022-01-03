using Astoneti.Microservice.AutoService.Models.Car;
using System.Collections.Generic;

namespace Astoneti.Microservice.AutoService.Models.Owner
{
    public class OwnerModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<CarModel> Cars { get; set; }
    }
}
