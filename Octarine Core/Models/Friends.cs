﻿
using System.Text.Json.Serialization;
using Octarine_Core.Autorisation;
using Octarine_Core.Classic;
using Octarine_Core.Resource.UsersIntefeces;

namespace Octarine_Core.Models
{
    internal class Friends
    {
        [JsonPropertyName("friendId")]
        public int FriendId { get; set; }

        [JsonPropertyName("friendName")]
        public string FriendName { get; set; }

        [JsonPropertyName("friendStatus")]
        public string FriendStatus { get; set; }
        [JsonPropertyName("friendPhoto")]
        public string  Foto {  get; set; }
        public SearchingFriend CreateSearchBrick(ChatController oc)
        {
            SearchingFriend sf = new SearchingFriend(oc,FriendId, FriendName, "В друзьях", Foto );
            return sf;
        }
        public AddUserToRoomBrick CreateUserBrick (AddFriendToRoomController ac)
        {
            AddUserToRoomBrick ab = new AddUserToRoomBrick(FriendName, FriendId, ac);
            return ab;
        }
    }
}
