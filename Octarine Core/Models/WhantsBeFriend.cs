

using System.Text.Json.Serialization;
using Octarine_Core.Autorisation;
using Octarine_Core.Classic;
using Octarine_Core.Resource.UsersIntefeces;

namespace Octarine_Core.Models
{
    internal class WhantsBeFriend
    {
        [JsonPropertyName("friendId")]
        public int FriendId { get; set; }

        [JsonPropertyName("userId")]
        public int UserId { get; set; }

        [JsonPropertyName("userName")]
        public string UserName { get; set; }
        public SearchingFriend CreateAcceptBrick(ChatController oc)
        {
            SearchingFriend sf = new SearchingFriend(oc, UserId, UserName, "Хочет дружить)");
            return sf;
        }
    }
}
