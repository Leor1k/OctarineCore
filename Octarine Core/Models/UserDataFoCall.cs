using System.Text.Json.Serialization;

namespace Octarine_Core.Models
{
    public class UserDataFoCall
    {
        [JsonPropertyName("user_id")]
        public int Id { get; set; }
        [JsonPropertyName("username")]
        public string Username { get; set; }

    }
}
