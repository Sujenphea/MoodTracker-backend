using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace finalMoodTracker.Models
{
    public class User
    {
        [Key]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "gitHub")]
        public string GitHub { get; set; }

        [JsonProperty(PropertyName = "imageURI")]
        public string ImageURI { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public ICollection<Daily> Dailies { get; set; } = new List<Daily>();
    }
}