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

        public string ChatName { get; private set; } = string.Empty;
        [JsonPropertyName("participants")]
        public List<UserDto> Participants
        {
            get => _participants;
            set
            {
                _participants = value;
                ChatName = string.Empty; // Очищаем имя чата перед обновлением
                foreach (var item in value)
                {
                    if (item.UserId != Properties.Settings.Default.UserID)
                    {
                        ChatName += item.UserName + " ";
                        Console.WriteLine(ChatName);
                    }
                }
                ChatName = ChatName.Trim(); // Убираем лишний пробел в конце
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
