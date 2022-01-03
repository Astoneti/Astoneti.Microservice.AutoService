namespace Astoneti.Microservice.AutoService.Models.Car
{
    public class CarModel
    {
        public int Id { get; set; }

        public string CarBrand { get; set; }

        public string Model { get; set; }

        public string LicensePlate { get; set; }

        public int? OwnerId { get; set; }

        public CarOwnerModel Owner { get; set; }
    }
}
