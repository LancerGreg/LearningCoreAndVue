using System;
using System.Text.Json.Serialization;

namespace backend.Models
{
    public class Chat
    {
        [JsonPropertyName("Id")]
        public Guid Id { get; set; }
        [JsonPropertyName("Name")]
        public string Name { get; set; }
        [JsonPropertyName("DateCreate")]
        public DateTime DateCreate { get; set; }
    }
}
