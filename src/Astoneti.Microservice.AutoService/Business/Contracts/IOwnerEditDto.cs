namespace Astoneti.Microservice.AutoService.Business.Contracts
{
    public interface IOwnerEditDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        // todo: think about list of car ids?
        public int CarId { get; set; }
    }
}
