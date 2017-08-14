// Class base for entities properties
namespace VeiculosWebApi.Models
{
    public class EntityBase
    {
        public string Id { get; set;}

        public string CreateTimeStamp { get; set;}

        public string ModificationTimeStamp { get; set; }
        
        public bool Active { get; set;}
    }

    public class EntityBaseWithName : EntityBase
    {
        public string Name { get; set; }
    }
}