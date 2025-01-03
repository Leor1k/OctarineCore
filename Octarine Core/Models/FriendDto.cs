

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
        public SearchingFriend CreateSearchBrick()
        {
            SearchingFriend sf = new SearchingFriend(FriendId, FriendName, FriendStatus);
            return sf;
        }
    }
}
