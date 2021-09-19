using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace MoodTrackerBackendCosmos.Models
{
    public class Daily
    {
        [Key]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "dateCreated")]
        public string DateCreated { get; set; }

        [JsonProperty(PropertyName = "userId")]
        public string UserId { get; set; }

        [JsonIgnore]
        public User User { get; set; } = null!;
    }
}