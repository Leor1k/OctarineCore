
using System;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private OctarineWindow OcWi;
        public ChatController(StackPanel _stackPanel, OctarineWindow oc)
        {
            stackPanel = _stackPanel;
            OcWi = oc;
        }
        public async Task LoadChat(int ChatId)
        {
           
            ApiRequests ap = new ApiRequests();
            var messages = await ap.GetAsync<Message>($"{Properties.Settings.Default.LoadGroupChat}{ChatId}");
            if (messages != null)
            {
                foreach (var message in messages)
                {
                    DateTime datetime = Convert.ToDateTime(message.CreatedAt);
                    MessageBrick mb = new MessageBrick(message.Content, message.SenderName, datetime.ToString("H:mm dd/mm/yyyy"),message.SenderId);

                    if (message.SenderId == Properties.Settings.Default.UserID)
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
                    MessageBrick mb = new MessageBrick(message.Content, message.SenderName, datetime.ToString("H:mm dd/mm/yyyy"), message.SenderId);
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
                    await OcWi.ShowUsersChat(brick.FriendName, brick.FriendIds[0], brick);
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
            ApiRequests api = new ApiRequests();
            try
            {

                await api.PostAsync<object>(Properties.Settings.Default.CreatePrivateChate, CreateChatRequest);
                await OcWi.ShowUsersChat(FriendName, IdFriend, null);
            }
            catch
            {
                ErrorAutUIController errorAutUIController = new ErrorAutUIController();
                errorAutUIController.ShowUserError("Произошла непредвиденная ошибка при создании чата.", Properties.Settings.Default.BorderForEror, false);
            }
        }
        public async void AddUsersInChat(AddUserInChat request)
        {
            try
            {
                ApiRequests api = new ApiRequests();
                await api.PostAsync<object>(Properties.Settings.Default.AddUserinChat, request);
            }
            catch
            {
                ErrorAutUIController errorAutUIController = new ErrorAutUIController();
                errorAutUIController.ShowUserError("Произошла непредвиденная ошибка при создании чата.", Properties.Settings.Default.BorderForEror, false);
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
                var newChatBrick = new FriendBrick(response.ChatId, chat.ChatName, "Новый чат", chat.Particals);
                newChatBrick.ChatClicked += (sender, e) =>
                {
                    this.OnChatClick(sender, e);
                };

                OcWi.ChatStack.Children.Insert(0, newChatBrick);
                await OcWi.ShowGroupChat(chat.ChatName,chat.Particals,response.ChatId);
                Properties.Settings.Default.IdActiveChat = response.ChatId;
            }
            catch
            {
                ErrorAutUIController errorAutUIController = new ErrorAutUIController();
                errorAutUIController.ShowUserError("Произошла непредвиденная ошибка при создании чата.", Properties.Settings.Default.BorderForEror, false);
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
                    Content = Conent
                };
                await ap.PostAsync<object>(Properties.Settings.Default.PostApi, MessageRuqest);

            }
            catch (Exception ex)
            {
                
            }

        }
        public async Task DeleteChat (int ChatId)
        {
            try
            {
                var api = new ApiRequests();
                await api.DeleteAsync<string>($"{Properties.Settings.Default.DeleteChat}{ChatId}");
                DeleteChatBrick(ChatId);
                var error = new ErrorAutUIController();
                error.ShowUserError("Чат успешно удалён", Properties.Settings.Default.BorderForEror, true);
                OcWi.ShowInfoBorder();
            }
            catch
            {
                var error = new ErrorAutUIController();
                error.ShowUserError("Произошла непредвиденная ошибка при удалении чата", Properties.Settings.Default.BorderForEror, false);
            }
        }
        public async Task DeleteUserFromChat (int chatId, int[] usesId)
        {
            try
            {
                var api = new ApiRequests();
                var request = new
                {
                    ChatId = chatId,
                    UserID = usesId
                };
                await api.DeleteAsyncWithRequest(Properties.Settings.Default.DeleteUserFromChat,request);
                if(usesId.Contains(Properties.Settings.Default.UserID))
                {
                    DeleteChatBrick(chatId);
                    OcWi.ShowInfoBorder();
                    var error = new ErrorAutUIController();
                    error.ShowUserError("Вы покинули чат", Properties.Settings.Default.BorderForEror, true);
                }
                else
                {
                    foreach (var bar in OcWi.IngoGrid.Children)
                    {
                        if (bar.GetType() == typeof(ChatUpBur))
                        {
                            var chatUpBur = (ChatUpBur) bar;
                            chatUpBur.RemoveFromUserList(usesId[0]);
                        }
                    }
                    var error = new ErrorAutUIController();
                    error.ShowUserError("Участник успешно удален", Properties.Settings.Default.BorderForEror, true);
                }
            }
            catch
            {
                var error = new ErrorAutUIController();
                error.ShowUserError("Произошла непредвиденная ошибка при попытке покинуть чат", Properties.Settings.Default.BorderForEror, false);
            }
        }
        public void DeleteChatBrick(int ChatId)
        {
            foreach (FriendBrick brick in OcWi.ChatStack.Children)
            {
                if (brick.ChatId == ChatId)
                {
                    OcWi.ChatStack.Children.Remove(brick);
                    break;
                }
            }
           
        }
        public void DeteChatBtickByFriendIf (int FriendId)
        {
            foreach (FriendBrick brick in OcWi.ChatStack.Children)
            {
                if (brick.FriendIds[0] == FriendId && brick.FriendIds.Length == 1)
                {
                    OcWi.ChatStack.Children.Remove(brick);
                    break;
                }
            }
        }

        public Grid ReturnMoreUiGrid()
        {
            return OcWi.MainGrid;
        }
    }
}
