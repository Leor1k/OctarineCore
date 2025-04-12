using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Octarine_Core.Resource.UsersIntefeces;

namespace Octarine_Core.Models
{
    internal class ChatDto
    {
        private List<UserDto> _participants = new List<UserDto>();

        [JsonPropertyName("chatId")]
        public int ChatId { get; set; }
        [JsonPropertyName("chatType")]
        public string ChatType { get; set; }
        [JsonPropertyName("chatName")]
        public string ChatName { get;  set; }

        [JsonPropertyName("participants")]
        public List<UserDto> Participants
        {
            get => _participants;
            set
            {
                if (ChatType == "private")
                {
                    _participants = value;
                    ChatName = string.Empty;

                    for (int i = _participants.Count - 1; i >= 0; i--)
                    {
                        if (_participants[i].UserId == Properties.Settings.Default.UserID)
                        {
                            _participants.RemoveAt(i);
                        }
                        else
                        {
                            ChatName += _participants[i].UserName + " ";
                        }
                    }

                    ChatName = ChatName.Trim();
                }
                else
                {
                    _participants = value;

                    for (int i = _participants.Count - 1; i >= 0; i--)
                    {
                        if (_participants[i].UserId == Properties.Settings.Default.UserID)
                        {
                            _participants.RemoveAt(i);
                        }
                    }
                }
               
            }
        }


        public FriendBrick CreateChatBrick ()
        {
            int[] FriendsId = new int[_participants.Count];
            for(int i = 0; i < _participants.Count; i++)
            {
                FriendsId[i] = _participants[i].UserId;
            }
            FriendBrick friendBrick = new FriendBrick(ChatId,ChatName,"В разработке", FriendsId);
            return friendBrick;
        }
    }

}
