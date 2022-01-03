namespace Astoneti.Microservice.AutoService.Business.Models
{
    public class CarDto
    {
        public int Id { get; set; }

        public string CarBrand { get; set; }

        public string Model { get; set; }

        public string LicensePlate { get; set; }

        public int? OwnerId { get; set; }

        public OwnerDto Owner { get; set; }
    }
}
