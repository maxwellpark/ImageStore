using System.Text.Json.Serialization;

namespace ImageStore.Data.Models
{
    public class ImageUploadResult
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("creationDate")]
        public string CreationDate { get; set; }

        public ImageUploadResult(string message, string creationDate)
        {
            Message = message;
            CreationDate = creationDate;
        }
    }
}
