

using Octarine_Core.Autorisation;
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
        public SearchingFriend CreateSearchBrick(OctarineWindow oc)
        {
            SearchingFriend sf = new SearchingFriend(oc,FriendId, FriendName, FriendStatus);
            return sf;
        }
    }
}
