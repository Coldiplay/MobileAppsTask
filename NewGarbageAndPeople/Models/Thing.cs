using System.Text.Json.Serialization;

namespace NewGarbageAndPeople.Models
{
    public class Thing
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public int? OwnerId { get; set; }
        public byte Count { get; set; } = 1;
        public bool Carryable { get; set; } = true;

        [JsonIgnore]
        public Owner? Owner { get; set; }
        [JsonIgnore]
        public string GetCarryable => Carryable ? "Пригодный для переноски" : "Непригодный для переноски";
    }
}
