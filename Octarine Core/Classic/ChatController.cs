using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Octarine_Core.Apis;
using Octarine_Core.Autorisation;
using Octarine_Core.Models;
using Octarine_Core.Resource.UsersIntefeces;

namespace Octarine_Core.Classic
{
    public class ChatController
    {
        private StackPanel stackPanel;
        public OctarineWindow OcWi;
        public ChatController(StackPanel _stackPanel, OctarineWindow oc)
        {
            stackPanel = _stackPanel;
            OcWi = oc;
        }
        public async Task LoadChat(int userID,int friendID)
        {
            ApiRequests ap = new ApiRequests();
            var messages = await ap.GetAsync<Message>($"{Properties.Settings.Default.GetMessagies}/{userID}/{friendID}");
            if (messages != null)
            {
                foreach (var message in messages)
                {
                   bool Friend = false;
                   if (message.SenderId != userID)
                   {
                        Friend = true;
                   }
                    MessageBrick mb = new MessageBrick(message.Content, Friend);
                    if (Friend ==false)
                    {
                        mb.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                    }
                    else
                    {
                        mb.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    }

                    stackPanel.Children.Add(mb);
                }
            }
        }
        public async void OnChatClick(object sender, EventArgs e)
        {
            if (sender is FriendBrick brick)
            {
                await OcWi.ShowUsersChat(brick.FriendName, brick.FriendIds[0]);
            }
        }
        public async void CreateChats(int IdUserm, int IdFriend, string FriendName)
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
                await OcWi.ShowUsersChat(FriendName,IdFriend);
            }
        }
    }
}
