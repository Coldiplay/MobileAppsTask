using System.Text.Json.Serialization;

namespace NewGarbageAndPeople.Models
{
    public class Owner
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string PhoneNumber { get; set; } = "";

        [JsonIgnore]
        public string FL => $"{FirstName} {LastName}";

        [JsonIgnore]
        public List<Thing> Things { get; set; } = new List<Thing>();

    }
}
