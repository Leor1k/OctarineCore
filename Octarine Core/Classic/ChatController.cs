
using System;
using System.Data.Common;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Octarine_Core.Apis;
using Octarine_Core.Autorisation;
using Octarine_Core.Models;
using Octarine_Core.Resource.UsersIntefeces;

namespace Octarine_Core.Classic
{
    public class ChatController
    {
        private StackPanel stackPanel;
        private OctarineWindow OcWi;
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
                    DateTime datetime = Convert.ToDateTime(message.CreatedAt);
                    MessageBrick mb = new MessageBrick(message.Content, Friend, datetime.ToString("H:mm dd/mm/yyyy"),message.SenderId);

                    if (Friend ==false)
                    {
                        mb.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                    }
                    else
                    {
                        mb.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    }
                    stackPanel.Children.Add(mb);
                    Properties.Settings.Default.IdActiveChat = message.ChatId;
                }
            }
        }
        public async Task LoadChatGroup(int ChatId)
        {

            ApiRequests ap = new ApiRequests();
            string a = $"{Properties.Settings.Default.LoadGroupChat}{ChatId}";
            var messages = await ap.GetAsync<Message>(a);
            if (messages != null)
            {
                foreach (var message in messages)
                {
                    bool Friend = false;
                    if (message.SenderId != Properties.Settings.Default.UserID)
                    {
                        Friend = true;
                    }
                    DateTime datetime = Convert.ToDateTime(message.CreatedAt);
                    MessageBrick mb = new MessageBrick(message.Content, Friend, datetime.ToString("H:mm dd/mm/yyyy"), message.SenderId);

                    if (Friend == false)
                    {
                        mb.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                    }
                    else
                    {
                        mb.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    }
                    stackPanel.Children.Add(mb);
                    Properties.Settings.Default.IdActiveChat = message.ChatId;
                }
            }
        }

        public async void OnChatClick(object sender, EventArgs e)
        {
            if (sender is FriendBrick brick)
            {
                if(brick.FriendIds.Length >1)
                {
                    await OcWi.ShowGroupChat(brick.FriendName, brick.FriendIds, brick.ChatId);
                }
                else
                {
                    await OcWi.ShowUsersChat(brick.FriendName, brick.FriendIds[0]);
                }
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
                await OcWi.ShowUsersChat(FriendName, IdFriend);
            }
            catch
            {
                ErrorAutUIController errorAutUIController = new ErrorAutUIController();
                errorAutUIController.ShowUserError("Произошла непредвиденная ошибка при создании чата.", Properties.Settings.Default.BorderForEror);
            }
        }
        public async void CreateGroupChat (GroupChat chat)
        {
            var CreateChatRequest = new
            {
                CreatorID = chat.CreatorId,
                UsersId = chat.Particals,
                ChatName = chat.ChatName,
            };
            try
            {
                ApiRequests api = new ApiRequests();
                CreatedGroupChatDTO response = await api.PostAsyncWithAnswer<object, CreatedGroupChatDTO>(Properties.Settings.Default.CreateGroupChatId, CreateChatRequest);
                response.ShowChat();
                MessageBox.Show($"А имя его: {chat.ChatName}"); 
                //await OcWi.ShowUsersChat(FriendName, IdFriend);
            }
            catch
            {
                ErrorAutUIController errorAutUIController = new ErrorAutUIController();
                errorAutUIController.ShowUserError("Произошла непредвиденная ошибка при создании чата.", Properties.Settings.Default.BorderForEror);
            }
        }
        public async Task SendMessageAsync(int FriendId, string Conent)
        {
            try
            {
                ApiRequests ap = new ApiRequests();
                var MessageRuqest = new
                {
                    ChatId = Properties.Settings.Default.IdActiveChat,
                    SenderId = Properties.Settings.Default.UserID,
                    ReceiverId = FriendId,
                    Content = Conent
                };
                await ap.PostAsync<object>(Properties.Settings.Default.PostApi, MessageRuqest);

            }
            catch (Exception ex)
            {
                
            }

        }

    }
}
