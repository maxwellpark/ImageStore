using System.Text.Json.Serialization;

namespace ImageStore.Data.Models
{
    public class ImageUploadResult
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("creationDate")]
        public string CreationDate { get; set; }

        public ImageUploadResult(string type, string message, string creationDate)
        {
            Type = type;
            Message = message;
            CreationDate = creationDate;
        }

        public ImageUploadResult(string type, string message)
        {
            Type = type;
            Message = message;
        }
    }
}
