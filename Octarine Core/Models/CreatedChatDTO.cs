
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Documents;

namespace Octarine_Core.Models
{
    public class CreatedChatDTO
    {
        [JsonPropertyName("chatId")]
        public int ChatId { get; set; }
        [JsonPropertyName("chattype")]
        public string ChatType { get; set; }
    }
    public class CreatedGroupChatDTO
    {
        [JsonPropertyName("chatID")]
        public int ChatId { get; set; }
        [JsonPropertyName("creatorId")]
        public int CreatorId { get; set; }
        public void ShowChat()
        {
            MessageBox.Show($"Чат: {ChatId} и создал его: {CreatorId} с именем:");
        }
    }
    public class GroupChat
    {
        public int CreatorId { get; set; } = Properties.Settings.Default.UserID;
        public int[] Particals { get; set; }
        public string ChatName {  get; set; }
    }
    public class AddUserInChat
    {
        public int ChatId { get; set; }
        public int[] UsersId { get; set; }
    }
}
