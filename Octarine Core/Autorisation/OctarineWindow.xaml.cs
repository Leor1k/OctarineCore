using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shell;
using Octarine_Core.Apis;
using Octarine_Core.Classic;
using Octarine_Core.Models;
using Octarine_Core.Resource.UsersIntefeces;

namespace Octarine_Core.Autorisation
{
    public partial class OctarineWindow : Window
    {
        private EnteredUserLite EnteredUserData;
        private FormConroller formc;
        private ChatController _chatController;
        private ChatHub _chatHub;
        private CallingController _callingController;
        private ErrorAutUIController _errorAutUIController;
        private SettingController _settingController;
        private HotKeyController _hotKeyController;
        public OctarineWindow()
        {
            InitializeComponent();
            WindowChrome.SetWindowChrome(this, new WindowChrome());
            LoadUserData();
            InitializeChatHub();
            LoadControllers();
            MainChatStack.SizeChanged += MainChatStack_SizeChanged;
            LoadProperiries();
        }

        private void LoadControllers ()
        {
            FormConroller ff = new FormConroller(MainGrid, SettingsGrid);
            formc = ff;
            formc.SwitchOctarineBorder(InfoBorder);
            _chatController = new ChatController(MainChatStack, this);
            _errorAutUIController = new ErrorAutUIController(FirstErorrGrid);
            _settingController = new SettingController(this);
            _hotKeyController = new HotKeyController(this);
        }
        private void LoadProperiries ()
        {
            Properties.Settings.Default.InColling = false;
            Properties.Settings.Default.BorderForEror = FirstErorrGrid;
            Properties.Settings.Default.ChatController = _chatController;
        }
        private async void InitializeChatHub()
        {
            _chatHub = new ChatHub(MainChatStack, _chatController);
            _chatHub._stackPanelForNewRequestFriends = FriendsAllStack;
            await _chatHub.StartConnectionAsync();
            _callingController = new CallingController(this);
            await _callingController.StartConnectionAsync();
        }
        private async void LoadUserData()
        {
            EnteredUserData = JWT.GetUserNameFromToken(Properties.Settings.Default.JwtToken.ToString());
            Properties.Settings.Default.UserID = EnteredUserData.GetIdUser();
            EnteredUserData.LoadUserBrick(UsersEnteredBrick);
            await LoadLastChats();
            await LoadUsersFriendRequests();
            await LoadUesrsFriends();
            LoadUesrsSettings();

        }
        private async Task LoadUsersFriendRequests()
        {
            ApiRequests ap = new ApiRequests();
            var friends = await ap.GetAsync<WhantsBeFriend>(EnteredUserData.GetFriendsRequestApi());
            if(friends!= null)
            {
                foreach (var friend in friends)
                {
                    FriendsAllStack.Children.Add(friend.CreateAcceptBrick(_chatController));
                }
            }
        }
        private async Task LoadUesrsFriends ()
        {
            ApiRequests ap = new ApiRequests();
            var friends = await ap.GetAsync<Friends>(EnteredUserData.GetPersonalApi());
            if (friends != null)
            {
                foreach (var friend in friends)
                {
                    FriendsAllStack.Children.Add(friend.CreateSearchBrick(_chatController));
                }
            }   
        }
        private void LoadUesrsSettings()
        {
            UsersImageSettings.Source = UsersEnteredBrick.UsersImage.Source;
            ChangeNameTb.Text = UsersEnteredBrick.UserNameTx.Text;
        }
        private async Task LoadLastChats()
        {
            ApiRequests ap = new ApiRequests();
            var Chats = await ap.GetAsync<ChatDto>(Properties.Settings.Default.GetChats + EnteredUserData.GetIdUser());
            if (Chats != null)
            {
                foreach (ChatDto chat in Chats)
                {
                    if(chat.ChatName != string.Empty)
                    {
                        FriendBrick brick = chat.CreateChatBrick();
                        brick.ChatClicked += (sender, e) =>
                        {
                            _chatController.OnChatClick(sender, e);
                        };
                        ChatStack.Children.Add(brick);
                    }
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }
        private void UpBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SizeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }

        private void HideBtn_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void SearchFriendBtn_Click(object sender, RoutedEventArgs e)
        {
            formc.SwitchOctarineBorder(SearchFrindBrd);
        }
        
        private async void SearchBt_Click(object sender, RoutedEventArgs e)
        {
            await SearchUser(Convert.ToInt32(SearchingUserIdTb.Text));
        }
        private async Task SearchUser(int id)
        {
            ApiRequests ap = new ApiRequests();
            var friends = await ap.GetAsync<FriendDto>(Properties.Settings.Default.SearchFriend + $"{UsersEnteredBrick.IdUser}&userIds={id}");
            SearchStack.Children.Clear();
            if (friends != null)
            {
                foreach (var friend in friends)
                {
                    SearchStack.Children.Add(friend.CreateSearchBrick(_chatController));
                }
            }
            else
            {
                _errorAutUIController.ShowUserError("Пользователей с таким ID не найдено", FirstErorrGrid, false);
            }
           
        }
        private void NumberOnlyImput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
                return;
            }
        }

        private void YourFriendBtn_Click(object sender, RoutedEventArgs e)
        {
            SearchFriendGrd.Visibility = Visibility.Hidden;
            AllFriendsGrid.Visibility = Visibility.Visible;
        }

        private void SearchYourFriendBtn_Click(object sender, RoutedEventArgs e)
        {
            SearchFriendGrd.Visibility = Visibility.Visible;
            AllFriendsGrid.Visibility = Visibility.Hidden;
        }
        public async Task ShowUsersChat(string FriendName, int friendID, FriendBrick brick)
        {
            formc.SwitchOctarineBorder(ChatWindow);
            MainChatStack.Children.Clear();

            FriendBrick dd = null;

            foreach (FriendBrick fb in ChatStack.Children)
            {
                if (fb.FriendName == FriendName)
                {
                    dd = fb;
                    break;
                }
            }

            if (dd == null) 
            {
                ApiRequests ap = new ApiRequests();
                var chatID = await ap.GetAsyncNoList<int>($"{Properties.Settings.Default.GetChatID}/{Properties.Settings.Default.UserID}/{friendID}");
                int ChatIdInt = Convert.ToInt32(chatID);

                int[] chatParitkansId = { friendID};
                dd = new FriendBrick(ChatIdInt, FriendName, "В разработке", chatParitkansId);

                Properties.Settings.Default.IdActiveChat = ChatIdInt;
                dd.ChatClicked += (sender, e) =>
                {
                    _chatController.OnChatClick(sender, e);
                };
                ChatStack.Children.Add(dd);
                
            }
            int id = 0;
            if (Properties.Settings.Default.InColling == false)
            {
                IngoGrid.Children.Clear();
                ChatUpBur cb = new ChatUpBur(FriendName, friendID, _callingController, dd);
                id = Convert.ToInt32(cb.ChatId);
                IngoGrid.Children.Add(cb);
            }
            if (id == 0)
            {
                await _chatController.LoadChat(brick.ChatId);
                Properties.Settings.Default.IdActiveChat = brick.ChatId;
            }
            else
            {
                await _chatController.LoadChat(id);
                Properties.Settings.Default.IdActiveChat =id;

            }
            Properties.Settings.Default.FriendId = friendID;
        }
        public async Task ShowGroupChat(string RoomName, int[] friendIDs, int chatID)
        {
            formc.SwitchOctarineBorder(ChatWindow);
            MainChatStack.Children.Clear();
            if (Properties.Settings.Default.InColling == false)
            {
                IngoGrid.Children.Clear();
                ChatUpBur cb = new ChatUpBur(RoomName, friendIDs, _callingController, chatID);
                IngoGrid.Children.Add(cb);
            }
            await _chatController.LoadChatGroup(chatID);
            Properties.Settings.Default.FriendId = friendIDs[0];
            Properties.Settings.Default.IdActiveChat = chatID;
            Scr.ScrollToEnd();
        }

        private void MainChatStack_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var scrollViewer = FindVisualChild<ScrollViewer>(MainChatStack);
            if (scrollViewer != null)
            {
                scrollViewer.ScrollToEnd();
            }
        }
        private static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                if (child is T result)
                {
                    return result;
                }
                var childResult = FindVisualChild<T>(child);
                if (childResult != null)
                {
                    return childResult;
                }
            }
            return null;
        }

        private async void SendMessage()
        {
            if (SendedMesageTb.Text.Trim().Length != 0)
            {
                try
                {
                    await _chatController.SendMessageAsync(Properties.Settings.Default.FriendId, SendedMesageTb.Text.Trim());

                    MessageBrick newMes = new MessageBrick(SendedMesageTb.Text, Properties.Settings.Default.UserName, DateTime.Now.ToString("H:mm dd/mm/yyyy"), Properties.Settings.Default.UserID);
                    newMes.HorizontalAlignment = HorizontalAlignment.Right;
                    MainChatStack.Children.Add(newMes);
                    SendedMesageTb.Clear();
                    Scr.ScrollToEnd();
                }
                catch
                {

                }
            }
        }
        private  void SendMessageBtn_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        private void SendedMesageTb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage();
            }
        }

        private void OpenSettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            settingsBorder.Visibility = Visibility.Visible;
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (ChangeNameTb.Text != UsersEnteredBrick.UserName)
            {
                 _settingController.ChangeUserName(ChangeNameTb.Text.Trim(), Properties.Settings.Default.UserID);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            settingsBorder.Visibility = Visibility.Hidden;
        }
        public void ShowInfoBorder()
        {
            formc.SwitchOctarineBorder(InfoBorder);
        }

        private async void ChanheImageBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MinIO minio = new MinIO();
                await minio.UploadUserAvatar(EnteredUserData.GetIdUser().ToString());

                var image = await minio.LoadImageFromMinIO($"IconUser{EnteredUserData.GetIdUser().ToString()}");
                if (image != null)
                {
                    UsersEnteredBrick.UsersImage.Source = image;
                    UsersImageSettings.Source = image;
                }
                _errorAutUIController.ShowUserError("Ваш аватар успешно изменён", true);
            }
            catch
            {
                _errorAutUIController.ShowUserError("Произошла непредвиденная ошибка", false);
            }
           
                
        }

        private void AcShow_Click(object sender, RoutedEventArgs e)
        {
            formc.SwitchSettingGrid(AccauntGrid);
        }

        private void HotKeyShow_Click(object sender, RoutedEventArgs e)
        {
            formc.SwitchSettingGrid(HotKeyGrid);
        }
    }
}
