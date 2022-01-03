namespace Astoneti.Microservice.AutoService.Business.Contracts
{
    public interface ICarEditDto
    {
        public int Id { get; set; }

        public string CarBrand { get; set; }

        public string Model { get; set; }

        public string LicensePlate { get; set; }

        public int OwnerId { get; set; }
    }
}
