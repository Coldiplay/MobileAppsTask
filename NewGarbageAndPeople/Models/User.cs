using System.Text.Json.Serialization;

namespace NewGarbageAndPeople.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        //[JsonIgnore]
        //List<Owner> Owners { get; set; } = [];
        //[JsonIgnore]
        //List<Thing> Things { get; set; } = [];
        //List<FileClass> Files { get; set; } = [];
    }
}
