using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace CSharpApp.API.Models
{
    public class PostRecordRequestModel
    {
        [JsonProperty("userId")]
        public int UserId { get; set; }

        [JsonProperty("title")]
        public string? Title { get; set; }

        [JsonProperty("body")]
        public string? Body { get; set; }
    }
}