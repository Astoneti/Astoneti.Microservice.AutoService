namespace Astoneti.Microservice.AutoService.Data.Entities
{
    public class CarEntity
    {
        public int Id { get; set; }

        public string CarBrand { get; set; }

        public string Model { get; set; }

        public string LicensePlate { get; set; }

        public int? OwnerId { get; set; }

        public OwnerEntity Owner { get; set; }
    }
}
