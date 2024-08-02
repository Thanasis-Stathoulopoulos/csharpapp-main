using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace CSharpApp.API.Models
{
    public class PostRecordRequestModel
    {
        [JsonProperty("userId")]
        [Required]
        public int UserId { get; set; }

        [JsonProperty("title")]
        [Required]
        public string Title { get; set; }

        [JsonProperty("body")]
        [Required]
        public string Body { get; set; }
    }
}