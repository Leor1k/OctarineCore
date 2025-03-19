

using Octarine_Core.Autorisation;
using Octarine_Core.Classic;
using Octarine_Core.Resource.UsersIntefeces;
using System.Text.Json.Serialization;

namespace Octarine_Core.Models
{
    internal class FriendDto
    {
        [JsonPropertyName("id")]
        public int FriendId { get; set; }

        [JsonPropertyName("name")]
        public string FriendName { get; set; }

        [JsonPropertyName("status")]
        public string FriendStatus { get; set; }
        [JsonPropertyName("photoName")]
        public string PhotoName { get; set; }

        public SearchingFriend CreateSearchBrick(ChatController ch) 
        {
            SearchingFriend sf = new SearchingFriend(ch, FriendId, FriendName, FriendStatus, PhotoName);
            return sf;
        }
    }
}
