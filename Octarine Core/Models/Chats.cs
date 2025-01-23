
using Octarine_Core.Apis;

namespace Octarine_Core.Models
{
    public class Chats
    {
        public async void CreateChats(int IdUserm, int IdFriend)
        {
            var CreateChatRequest = new
            {
                UserId1 = IdUserm,
                UserId2 = IdFriend,
            };
            try
            {
                ApiRequests api = new ApiRequests();
                await api.PostAsync<object>(Properties.Settings.Default.CreatePrivateChate, CreateChatRequest);
            }
            catch
            {

            }
            finally
            {

            }
        }
    }
}
